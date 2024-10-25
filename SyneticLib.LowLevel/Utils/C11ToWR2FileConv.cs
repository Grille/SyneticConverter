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
using System.Drawing;
using System.Reflection;

namespace SyneticLib.Utils;

public static class C11ToWR2FileConv
{
    static readonly TextureTransform MatrixUV;

    static readonly TextureTransform Matrix64;
    static readonly TextureTransform Matrix128;
    static readonly TextureTransform Matrix256;
    static readonly TextureTransform Matrix512;

    static C11ToWR2FileConv()
    {
        MatrixUV = TextureTransform.CreateScale90Deg(1, 1);
        Matrix64 = TextureTransform.CreateScale(1f / 64f, 1f / 64f);
        Matrix128 = TextureTransform.CreateScale(1f / 128f, 1f / 128f);
        Matrix256 = TextureTransform.CreateScale(1f / 256f, 1f / 256f);
        Matrix512 = TextureTransform.CreateScale(1f / 512f, 1f / 512f);
    }

    public static void Convert(string dirPath, string fileName)
    {
        var paths = new ScenarioFiles.Paths(dirPath, fileName);
        var files = new ScenarioFiles();
        files.Load(paths, GameVersion.C11);

        ConvertGeo(files.TerrainMesh.Vertices);
        ConvertQad(files.Qad);

        WR2CobFileCreator.CreateCobFiles(dirPath);

        ConvertObjectMaterials(Path.Combine(dirPath, "Objects"));

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
            float min = MathF.Min(MathF.Min(vertex.LightColor.X, vertex.LightColor.Y), vertex.LightColor.Z);
            var minvec = new Vector3(min + 0.1f);
            vertex.LightColor = Vector3.ComponentMin(vertex.LightColor, minvec);
            if (min > 0.6)
            {
                vertex.LightColor = vertex.LightColor * 0.25f + new Vector3(0.45f);
            }
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

            TerrainMaterialMapper.ConvertC11ToWR2(ref material, matrices);

            if (material.Layer0.Mode < 4 && material.Layer0.Tex0Id == fels07)
            {
                material.Layer0.Tex0Id = 0;
            }
        }

        qad.SetFlagsAccordingToVersion(GameVersion.WR2);
        qad.SortMaterials();
        qad.RecalcMaterialMatrixChecksum();
    }

    static TextureTransform MatrixFromName(string name)
    {
        var lname = name.ToLower();

        if (lname == "03gras" || lname == "nk03gras" || lname == "gras13" || lname == "gras14")
            return Matrix512;

        if (name.Contains("_S"))
            return Matrix512;

        if (lname.Contains("fels"))
            return Matrix256;
        if (lname == "wasserbump")
            return Matrix256;
        if (lname.Contains("acker") || lname.Contains("feld"))
            return Matrix128;

        return Matrix64;
    }

    static void ConvertObjectMaterials(string path)
    {
        foreach (var file in Directory.EnumerateFiles(path))
        {
            var extension = Path.GetExtension(file).ToLower();
            if (extension == ".mtl")
            {
                var mtl = new MtlFile();
                mtl.Load(file);
                foreach (var material in mtl.Sections.Values)
                {
                    ConvertObjectMaterial(material);
                }
                mtl.Save(file);
            }
        }
    }

    static void ConvertObjectMaterial(MtlFile.MtlMaterial material)
    {
        if (material.Alpha.TryGetObject(out var alpha))
        {
            material.Transparency.Object = alpha;
            material.Alpha.Exists = false;
        }

        material.Reflect.Value = material.Reflect2.Value;

        static Vector4 Input(int value) => (Vector4)(Color4)Color.FromArgb(value);

        var ambient = Input(material.Ambient.Object![0]);

        ambient *= 0.1f;

        material.Ambient.Object![0] = ((Color4)ambient).ToArgb();
        material.Ambient.Flush();
    }
}
