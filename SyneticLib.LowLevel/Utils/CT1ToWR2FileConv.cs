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
using static SyneticLib.Files.QadFile;

namespace SyneticLib.Utils;

public static class CT1ToWR2FileConv
{
    static readonly TextureTransform MatrixUV;

    static readonly TextureTransform Matrix64;
    static readonly TextureTransform Matrix128;
    static readonly TextureTransform Matrix512;

    static CT1ToWR2FileConv()
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
        files.Load(paths, GameVersion.CT1);

        ConvertGeo(files.TerrainMesh.Vertices);
        ConvertQad(files.Qad);

        foreach (var file in Directory.EnumerateFiles(dirPath))
        {
            var ext = Path.GetExtension(file).ToLower();
            if (ext == ".dyn" || ext == ".trg" || ext == ".wyp" || ext == ".trn" || ext == ".hli" || ext == ".par")
            {
                File.Delete(file);
            }
        }

        files.Save(paths, GameVersion.WR2);
    }

    public static void ConvertGeo(Vertex[] vertecis)
    {
        var factor = new Vector3(0.5f, 0.5f, 0.5f);
        var offset = new Vector3(0.1f);

        for (int i = 0; i < vertecis.Length; i++)
        {
            ref var vertex = ref vertecis[i];

            var blend = vertex.Blending;
            vertex.Blending = new Vector3(blend.Y, blend.Z, blend.X);
            //vertex.LightColor = Vector3.Clamp(vertex.LightColor * factor + offset, Vector3.Zero, Vector3.One);
        }
    }

    public static void ConvertQad(QadFile qad)
    {
        var matrices = new TextureTransform[qad.TextureNames.Length];
        for (int i = 0; i < matrices.Length; i++)
        {
            matrices[i] = MatrixFromName(qad.TextureNames[i]);
        }

        for (int i = 0; i < qad.Head.MaterialCount; i++)
        {
            ref var material = ref qad.Materials[i];

            TerrainMaterialMapper.ConvertC11ToWR2(ref material, matrices);
        }

        
        qad.Head.BumpTexturesFileCount = 0;
        qad.BumpTexNames = Array.Empty<String32>();

        qad.Head.PropInstanceCount = 0;
        qad.PropInstances = Array.Empty<MPropInstance>();

        for (int i = 0; i < qad.Chunks.Length; i++)
        {
            ref var chunk = ref qad.Chunks[i];

            chunk.Props.Start = 0;
            chunk.Props.Length = 0;
            
            chunk.Lights.Start = 0;
            chunk.Lights.Length = 0;
        }
        
        qad.Head.PropClassCount = 0;
        qad.PropClassObjNames = Array.Empty<String32>();
        qad.PropClassInfo = Array.Empty<MPropClass>();
        

        
        for (int i = 0; i< qad.PropInstances.Length; i++)
        {
             ref var instance = ref qad.PropInstances[i];
            instance.InShadow = 0;
        }
        

        qad.Head.SoundCount = 0;
        qad.Sounds = Array.Empty<MSound>();

        qad.Head.LightCount = 0;
        qad.Lights = Array.Empty<MLight>();

        qad.SetFlagsAccordingToVersion(GameVersion.WR2);
        qad.SortMaterials();
        qad.ForceUniqueChecksums();

        qad.Validate();
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
