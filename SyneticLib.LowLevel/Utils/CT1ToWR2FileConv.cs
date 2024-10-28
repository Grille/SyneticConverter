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

using WR2Type = SyneticLib.TerrainMaterialTypeWR2;

namespace SyneticLib.Utils;

public static class CT1ToWR2FileConv
{
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

        ConvertRo0(files.Ro1);
        ConvertRo0(files.Ro2);
        ConvertRo0(files.Ro3);
        ConvertRo0(files.Ro4);

        WR2CobFileCreator.CreateCobFiles(dirPath);
        C11ToWR2FileConv.ConvertObjectMaterials(Path.Combine(dirPath, "Objects"));
        TrxToMoxConverter.Convert(Path.Combine(dirPath, "Objects"));

        files.Save(paths, GameVersion.WR2);
    }

    public static void ConvertRo0(Ro0File ro0)
    {
        ro0.Head.X1 = 0;

        for (int i = 0; i < ro0.Grass.Length; i++)
        {
            ref var gras = ref ro0.Grass[i];

            gras.Color0 = (gras.Color0 + gras.Color1) / 2f;
        }
    }

    public static void ConvertGeo(Vertex[] vertecis)
    {
        var mul = new Vector3(0.6f, 0.5f, 0.5f);
        var offset = new Vector3(-0.1f);

        for (int i = 0; i < vertecis.Length; i++)
        {
            ref var vertex = ref vertecis[i];

            var blend = vertex.Blending;
            vertex.Blending = new Vector3(blend.Y, blend.Z, blend.X);
            vertex.LightColor = Vector3.Clamp((vertex.LightColor + offset) * mul, Vector3.Zero, Vector3.One);

            VertexColorCorrector.ClampToMax(ref vertex.LightColor, -0.25f);
        }
    }


    public unsafe static void ConvertQad(QadFile qad)
    {
        qad.Head.BumpTexturesFileCount = 0;
        qad.BumpTexNames = Array.Empty<String32>();

        var matrices = new TextureTransform[qad.TextureNames.Length];
        for (int i = 0; i < matrices.Length; i++)
        {
            matrices[i] = MatrixFromName(qad.TextureNames[i]);
        }

        ushort GetTexId(string name)
        {
            int idx = Array.FindIndex(qad.TextureNames, (a) => a == name);
            return idx > 0 ? (ushort)idx : throw new IndexOutOfRangeException();
        }

        ushort idxZebra = GetTexId("ASPH01_zebra");
        ushort idxRichtung = GetTexId("ASPH01_richtungen");
        ushort idxRand032 = GetTexId("Rand03_2");

        for (int i = 0; i < qad.Head.MaterialCount; i++)
        {
            ref var material = ref qad.Materials[i];

            TerrainMaterialMapper.ConvertC11ToWR2(ref material, matrices);

            ref var layer = ref material.Layer0;
            (var zOffset, var type) = layer.Mode.Decode<WR2Type>();
            if (type == WR2Type.L2SpecFaded)
            {
                if (layer.Tex2Id == idxZebra || layer.Tex2Id == idxRichtung)
                {
                    layer.Mode.Encode(zOffset, WR2Type.L2SpecOverlay);
                    layer.Tex1Id = layer.Tex2Id;
                }
                if (layer.Tex2Id == idxRand032)
                {
                    layer.Mode.Encode(zOffset, WR2Type.L2Road);
                    material.Matrix2 = TerrainMaterialMapper.Matrix64;
                }
            }
        }

        for (int i = 0; i < qad.PropInstances.Length; i++)
        {
            ref var instance = ref qad.PropInstances[i];
            var ct = Unsafe.As<MPropInstanceWR2, MPropInstanceCT1>(ref instance);

            instance.Matrix = ct.Matrix;

            var q = ct.Rotation;
            float angle = -2 * MathF.Atan2(q.Y, q.W);
            instance.Angl = angle;
            instance.Size = ct.Scale;
            instance.InShadow = ct.InShadow;
        }

        ConvertXTreeReferences(qad);

        qad.SetFlagsAccordingToVersion(GameVersion.WR2);
        qad.SortMaterials();
        qad.ForceUniqueChecksums();

        qad.Validate();
    }

    public static void ConvertXTreeReferences(QadFile qad)
    {
        for (int i = 0; i < qad.PropClassObjNames.Length; i++)
        {
            string name = qad.PropClassObjNames[i];
            if (name.StartsWith("X\\"))
            {
                qad.PropClassObjNames[i] = (String32)name.Substring(2);
                qad.PropClassInfo[i].Mode = 3;
            }
        }

        for (int i = 0; i < qad.PropInstances.Length; i++)
        {
            ref var instance = ref qad.PropInstances[i];
            string name = instance.Name;
            if (name.StartsWith("X\\"))
            {
                instance.Name = (String32)name.Substring(2);
            }
        }
    }

    static TextureTransform MatrixFromName(string name)
    {
        var lname = name.ToLower();

        if (lname == "03gras" || lname == "nk03gras" || lname == "gras13" || lname == "gras14" || lname == "schmu05")
            return TerrainMaterialMapper.Matrix512;

        if (lname == "sandgrube")
            return TerrainMaterialMapper.Matrix256;

        if (lname == "erde1" || lname == "steine1" || lname == "02_gras_1" || lname == "02gras_1")
            return TerrainMaterialMapper.Matrix128;

        if (name.Contains("_S"))
            return TerrainMaterialMapper.Matrix512;

        if (lname.Contains("fels"))
            return TerrainMaterialMapper.Matrix256;
        if (lname == "wasserbump")
            return TerrainMaterialMapper.Matrix256;
        if (lname.Contains("acker") || lname.Contains("feld"))
            return TerrainMaterialMapper.Matrix128;

        return TerrainMaterialMapper.Matrix64;
    }
}
