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
using static SyneticLib.Files.CpoFile;

namespace SyneticLib.Conversion;

public static class C11ToWR2FileConv
{
    static readonly TextureTransform MatrixUV;

    static readonly TextureTransform Matrix64;
    static readonly TextureTransform Matrix128;
    static readonly TextureTransform Matrix512;

    static C11ToWR2FileConv()
    {
        MatrixUV = TextureTransform.CreateScale90Deg(1, 1);
        Matrix64 = TextureTransform.CreateScale(1f / 64f, 1f / 64f);
        Matrix128 = TextureTransform.CreateScale(1f / 128f, 1f / 128f);
        Matrix512 = TextureTransform.CreateScale(1f / 512f, 1f / 512f);
    }

    public static void Convert(string dirPath, string fileName)
    {
        var paths = new ScenarioFiles.Paths(dirPath, fileName);
        var files = new ScenarioFiles();
        files.Load(paths, GameVersion.C11);

        ConvertGeo(files.TerrainMesh.Vertices);
        ConvertQad(files.Qad);

        CreateCobFiles(dirPath);

        files.Save(paths, GameVersion.WR2);
    }

    public static void ConvertGeo(Vertex[] vertecis)
    {
        var black = new Vector3(0.2f);

        for (int i = 0; i < vertecis.Length; i++)
        {
            ref var vertex = ref vertecis[i];

            float f = vertex.LightColor.Length;

            var blend = vertex.Blending;
            vertex.Blending = new Vector3(blend.Y, blend.Z, blend.X);
            vertex.LightColor = Vector3.Clamp(vertex.LightColor * f + black * (1 - f), Vector3.Zero, Vector3.One);
        }
    }

    public static void ConvertQad(QadFile qad)
    {
        var matrices = new TextureTransform[qad.TextureNames.Length];
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

        for (int i = 0; i < qad.Head.MaterialCount; i++)
        {
            ref var material = ref qad.Materials[i];

            ConvertC11ToWR2(ref material, matrices);

            if (material.Layer0.Mode < 4 && material.Layer0.Tex0Id == fels07)
            {
                material.Layer0.Tex0Id = 0;
            }
        }

        qad.SetFlagsAccordingToVersion(GameVersion.WR2);
        qad.SortMaterials();
        qad.RecalcMaterialMatrixChecksum();
    }

    public static void ConvertC11ToWR2(ref this QadFile.AbstractMaterialType mat, TextureTransform[] matrices)
    {
        var dmat0 = matrices[mat.Layer0.Tex0Id];
        var dmat1 = matrices[mat.Layer0.Tex1Id];
        var dmat2 = matrices[mat.Layer0.Tex2Id];

        mat.Matrix0 = MatrixUV;
        mat.Matrix1 = MatrixUV;
        mat.Matrix2 = MatrixUV;

        switch (mat.Layer1.Mode)
        {
            case 320:
            {
                mat.Layer0.Mode = TerrainMaterialTypeWR2.Water;
                mat.Matrix0 = dmat0;
                break;
            }
            case 304:
            case 288:
            {
                mat.Layer0.Mode = TerrainMaterialTypeWR2.AlphaClip;
                break;
            }
            case 256:
            case 240:
            {
                mat.Layer0.Mode = TerrainMaterialTypeWR2.Road1;
                break;
            }
            case 224:
            {
                mat.Layer0.Mode = TerrainMaterialTypeWR2.UVTerrain;
                mat.Matrix2 = dmat2;
                break;
            }
            case 192:
            {
                mat.Layer0.Mode = TerrainMaterialTypeWR2.Reflective;
                break;
            }
            case 128:
            case 112:
            case 64:
            {
                mat.Layer0.Mode = TerrainMaterialTypeWR2.UV;
                break;
            }
            case 32:
            {
                mat.Layer0.Mode = TerrainMaterialTypeWR2.UV;
                break;
            }
            case 18:
            case 16:
            {
                mat.Layer0.Mode = TerrainMaterialTypeWR2.UVTerrain;
                mat.Matrix2 = dmat2;
                break;
            }
            case 3:
            case 2:
            case 1:
            case 0:
            {
                mat.Layer0.Mode = mat.Layer1.Mode;
                mat.Matrix0 = dmat0;
                mat.Matrix1 = dmat1;
                mat.Matrix2 = dmat2;
                break;
            }
            default:
            {
                throw new NotImplementedException();
            }

        }

    }

    static TextureTransform MatrixFromName(string name)
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

                var cobFileName = Path.ChangeExtension(name, ".cob");
                var cobFilePath = Path.Combine(colliDir, cobFileName);

                var cpoFileName = Path.ChangeExtension(name, ".cpo");
                var cpoFilePath = Path.Combine(colliDir, cpoFileName);

                if (false && File.Exists(cpoFilePath))
                {
                    var cpo = new CpoFile();
                    cpo.Load(cpoFilePath);
                    var cob = CreateCobFile(cpo);
                    cob.Save(cobFilePath);
                }
                else
                {
                    var mox = new MoxFile();
                    mox.Load(moxFilePath);
                    var cob = CreateCobFile(mox);
                    cob.Save(cobFilePath);
                }

                sbNames.AppendLine(name);
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

    public static CobFile CreateCobFile(CpoFile cpo)
    {
        var cob = new CobFile();
        return cob;
    }
}
