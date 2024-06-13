using SyneticLib.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;

namespace SyneticLib.Conversion;

public static class C11ToWR2FileConv
{
    static readonly Transform MatrixUV;

    static readonly Transform Matrix64;
    static readonly Transform Matrix128;
    static readonly Transform Matrix512;

    static C11ToWR2FileConv()
    {
        MatrixUV = Transform.CreateScale90Deg(1, 1);
        Matrix64 = Transform.CreateScale(1f / 64f, 1f / 64f);
        Matrix128 = Transform.CreateScale(1f / 128f, 1f / 128f);
        Matrix512 = Transform.CreateScale(1f / 512f, 1f / 512f);
    }

    public static void Convert(string dirPath, string fileName)
    {
        var filePath = Path.Combine(dirPath, fileName);

        ConvertGeo(filePath);
        ConvertQad(filePath);

        CreateCobFiles(dirPath);
    }

    public static void ConvertGeo(string path)
    {
        string qeopath = Path.ChangeExtension(path, "geo");
        string idxpath = Path.ChangeExtension(path, "idx");
        string vtxpath = Path.ChangeExtension(path, "vtx");

        var geo = new GeoFile();
        var idx = new IdxFile();
        var vtx = new VtxFile();

        geo.Path = qeopath;
        idx.Path = idxpath;
        vtx.Path = vtxpath;

        geo.Load();

        idx.Indices = geo.Indices;
        vtx.Vertecis = geo.Vertecis;
        vtx.IndicesOffset = geo.IndicesOffset;

        var black = new Vector3(0.2f);

        for (int i = 0; i < vtx.Vertecis.Length; i++)
        {
            ref var vertex = ref vtx.Vertecis[i];

            float f = vertex.LightColor.Length;

            var blend = vertex.Blending;
            vertex.Blending = new Vector3(blend.Y, blend.Z, blend.X);
            vertex.LightColor = Vector3.Clamp(vertex.LightColor * f + black * (1 - f), Vector3.Zero, Vector3.One);

            
        }

        idx.Save();
        vtx.Save();

        File.Delete(qeopath);
    }

    public static void ConvertQad(string path)
    {
        string qadpath = Path.ChangeExtension(path, "qad");

        var qad = new QadFile();

        qad.Path = qadpath;

        qad.SetFlagsAccordingToVersion(GameVersion.C11);
        qad.Load();

        var matrices = new Transform[qad.TextureNames.Length];
        for (int i = 0; i < matrices.Length; i++)
        {
            matrices[i] = MatrixFromName(qad.TextureNames[i]);
        }

        int fels07 = -1;

        for (int i = 0; i < qad.TextureNames.Length; i++)
        {
            if (qad.TextureNames[i].ToString().ToLower() == "fels07")
            {
                fels07 = i;
                break;
            }
        }

        qad.MaterialsWR = new QadFile.MMaterialTypeWR[qad.Head.MaterialCount];
        for (int i = 0; i < qad.Head.MaterialCount; i++)
        {
            ref var src = ref qad.MaterialsCT[i];
            ref var dst = ref qad.MaterialsWR[i];

            ConvertC11ToWR2(ref src, ref dst, matrices);

            if (dst.Mode < 4 && dst.Tex0Id == fels07)
            {
                dst.Tex0Id = 0;
            }
        }

        qad.SetFlagsAccordingToVersion(GameVersion.WR2);
        qad.SortMaterials();
        qad.RecalcMaterialChecksum();
        qad.Save();
    }

    public static void ConvertC11ToWR2(ref this QadFile.MMaterialTypeCT src, ref QadFile.MMaterialTypeWR dst, Transform[] matrices)
    {
        dst.Tex0Id = src.L0Tex0Id;
        dst.Tex1Id = src.L0Tex1Id;
        dst.Tex2Id = src.L0Tex2Id;

        var dmat0 = matrices[dst.Tex0Id];
        var dmat1 = matrices[dst.Tex1Id];
        var dmat2 = matrices[dst.Tex2Id];

        dst.Matrix0 = MatrixUV;
        dst.Matrix1 = MatrixUV;
        dst.Matrix2 = MatrixUV;

        switch (src.L1Mode)
        {
            case 320:
            {
                dst.Mode = TerrainMaterialTypeWR2.Water;
                dst.Matrix0 = dmat0;
                break;
            }
            case 304:
            case 288:
            {
                dst.Mode = TerrainMaterialTypeWR2.AlphaClip;
                break;
            }
            case 256:
            case 240:
            {
                dst.Mode = TerrainMaterialTypeWR2.Road3;
                break;
            }
            case 224:
            {
                dst.Mode = TerrainMaterialTypeWR2.UVTerrain;
                dst.Matrix2 = dmat2;
                break;
            }
            case 192:
            {
                dst.Mode = TerrainMaterialTypeWR2.Reflective;
                break;
            }
            case 128:
            case 112:
            case 64:
            {
                dst.Mode = TerrainMaterialTypeWR2.UV;
                break;
            }
            case 32:
            {
                dst.Mode = TerrainMaterialTypeWR2.UV;
                break;
            }
            case 18:
            case 16:
            {
                dst.Mode = TerrainMaterialTypeWR2.UVTerrain;
                dst.Matrix2 = dmat2;
                break;
            }
            case 3:
            case 2:
            case 1:
            case 0:
            {
                dst.Mode = src.L1Mode;
                dst.Matrix0 = dmat0;
                dst.Matrix1 = dmat1;
                dst.Matrix2 = dmat2;
                break;
            }
            default:
            {
                throw new NotImplementedException();
            }

        }

    }

    static Transform MatrixFromName(string name)
    {
        var lname = name.ToLower();

        if (name.Contains("_S"))
            return Matrix512;

        if (lname.Contains("fels"))
            return Matrix128;
        if (lname == "wasserbump")
            return Matrix128;
        if (lname.Contains("acker") || lname.Contains("feld"))
            return Matrix128;

        return Matrix64;
    }

    public static void CreateCobFiles(string path)
    {
        var objectDir = Path.Combine(path, "Objects");
        var colliDir = Path.Combine(path, "Colli");
        Directory.CreateDirectory(colliDir);

        var sbNames = new StringBuilder();

        foreach (var moxFilePath in Directory.EnumerateFiles(objectDir))
        {
            if (Path.GetExtension(moxFilePath).ToLower() == ".mox")
            {
                var name = Path.GetFileNameWithoutExtension(moxFilePath);

                var mox = new MoxFile();
                mox.Load(moxFilePath);

                sbNames.AppendLine(name);

                var cobFileName = Path.ChangeExtension(name, ".cob");
                var cobFilePath = Path.Combine(colliDir, cobFileName);

                var cob = CreateCobFile(mox);
                cob.Save(cobFilePath);
            }
        }

        var namesFileName = Path.Combine(colliDir, "ColliList.txt");
        File.WriteAllText(namesFileName, sbNames.ToString());
    }

    public static CobFile CreateCobFile(MoxFile mox)
    {
        var cob = new CobFile();
        cob.Vertecis = mox.Vertecis;
        cob.Indices = mox.Indices;
        cob.Head.VerticeCount = cob.Vertecis.Length;
        cob.Head.PolyCount = cob.Indices.Length;
        cob.Head.BoundingBox = new BoundingBox(cob.Vertecis, cob.Indices);
        return cob;
    }
}
