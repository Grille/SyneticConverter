﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;
using System.Drawing;
using OpenTK.Mathematics;

using WR1IDX = SyneticLib.Files.MtlFile.WR1ColorIndex;
using WR2IDX = SyneticLib.Files.MtlFile.WR2ColorIndex;

namespace SyneticLib.Utils;

public static class ConvertMbwrCar
{
    public static IReadOnlyDictionary<int, int> ColorSwapDict;

    static ConvertMbwrCar()
    {
        ColorSwapDict = new Dictionary<int, int>(15)
        {
            {WR1IDX.Schwarz, WR2IDX.Schwarz },
            {WR1IDX.Rot, WR2IDX.Rot1 },
            {WR1IDX.DunkelRot, WR2IDX.Rot2 },
            {WR1IDX.Gelb, WR2IDX.Gelb1 },
            {WR1IDX.Gold, WR2IDX.DunkelRot },
            {WR1IDX.Weiss, WR2IDX.Weiss },
            {WR1IDX.Anthrazit, WR2IDX.Anthrazit },
            {WR1IDX.DunkelGruen, WR2IDX.DunkelGruen1 },
            {WR1IDX.Blau, WR2IDX.Blau1 },
            {WR1IDX.HellBlau, WR2IDX.Blau2 },
            {WR1IDX.QuarzBlau, WR2IDX.QuarzBlau },
            {WR1IDX.Gruen, WR2IDX.DunkelGruen2 },
            {WR1IDX.Silber, WR2IDX.Silber1 },
            {WR1IDX.Heliodor, WR2IDX.Gelb2 },
            {WR1IDX.Tuerkis, WR2IDX.Silber2 },
        };
    }

    public static void Convert(string dirPath, string fileName)
    {
        var driverDirPath = Path.Combine(dirPath, "driver");
        var cpitDirPath = Path.Combine(dirPath, "cpit");

        Directory.Move(Path.Combine(dirPath, "textures"), Path.Combine(dirPath, "textures_pc"));
        Directory.Move(Path.Combine(driverDirPath, "textures"), Path.Combine(driverDirPath, "textures_pc"));

        foreach (var file in Directory.EnumerateFiles(dirPath))
        {
            var name = Path.GetFileNameWithoutExtension(file).ToUpperInvariant();
            if (name.EndsWith("_LDT") || name.EndsWith("_SH"))
            {
                File.Delete(file);
            }
        }

        int usedDriver = 10;

        for (int i = 1; i <= 10; i++)
        {
            if (i == usedDriver)
            {
                continue;
            }

            File.Delete($"{driverDirPath}/{fileName}_dr{i:D2}_1.mox");
            File.Delete($"{driverDirPath}/{fileName}_dr{i:D2}_1.mtl");
            File.Delete($"{driverDirPath}/{fileName}_dr{i:D2}_2.mox");
            File.Delete($"{driverDirPath}/{fileName}_dr{i:D2}_3.mox");

            File.Delete($"{driverDirPath}/{fileName}_hd{i:D2}.mox");
            File.Delete($"{driverDirPath}/{fileName}_hd{i:D2}.mtl");

            File.Delete($"{driverDirPath}/textures_pc/dr{i:D2}.ptx");
            File.Delete($"{driverDirPath}/textures_pc/hd{i:D2}.ptx");

            File.Delete($"{cpitDirPath}/ARMS{i:D2}_{fileName}.mox");
            File.Delete($"{cpitDirPath}/ARMS{i:D2}_{fileName}.mtl");
        }

        File.Move($"{driverDirPath}/{fileName}_dr{usedDriver:D2}_1.mox", $"{driverDirPath}/{fileName}_dr{1:D2}_1.mox");
        File.Move($"{driverDirPath}/{fileName}_dr{usedDriver:D2}_1.mtl", $"{driverDirPath}/{fileName}_dr{1:D2}_1.mtl");
        File.Move($"{driverDirPath}/{fileName}_dr{usedDriver:D2}_2.mox", $"{driverDirPath}/{fileName}_dr{1:D2}_2.mox");
        File.Move($"{driverDirPath}/{fileName}_dr{usedDriver:D2}_3.mox", $"{driverDirPath}/{fileName}_dr{1:D2}_3.mox");

        File.Move($"{driverDirPath}/{fileName}_hd{usedDriver:D2}.mox", $"{driverDirPath}/{fileName}_hd{1:D2}.mox");
        File.Move($"{driverDirPath}/{fileName}_hd{usedDriver:D2}.mtl", $"{driverDirPath}/{fileName}_hd{1:D2}.mtl");

        File.Move($"{cpitDirPath}/ARMS{usedDriver:D2}_{fileName}.mox", $"{cpitDirPath}/ARMS{1:D2}_{fileName}.mox");
        File.Move($"{cpitDirPath}/ARMS{usedDriver:D2}_{fileName}.mtl", $"{cpitDirPath}/ARMS{1:D2}_{fileName}.mtl");

        Material($"{dirPath}/{fileName}.mtl");
        Material($"{cpitDirPath}/cpit_{fileName}.mtl");
    }

    static void Material(string path)
    {
        var mtl = new MtlFile();
        mtl.Load(path);
        Material(mtl);
        mtl.Save(path);
    }

    static void Material(MtlFile mtl)
    {
        foreach (var mat in mtl.Sections.Values)
        {
            Material(mat);
        }
    }

    static void Material(MtlFile.MtlMaterial mat)
    {
        var srcColors = mat.Colors;

        var diffuse = srcColors[0].Diffuse;
        bool equal = true;
        for (int i = 1; i < srcColors.Length; i++)
        {
            if (srcColors[i].Diffuse != diffuse)
            {
                equal = false;
                break;
            }
        }

        if (equal)
        {
            return;
        }

        if (srcColors.Length != 15)
        {
            throw new InvalidDataException();
        }

        var dstColors = new MtlFile.MtlColor[srcColors.Length];

        for (int isrc = 0; isrc < 15; isrc++)
        {
            dstColors[ColorSwapDict[isrc]] = srcColors[isrc];
        }

        for (int i = 0; i < 15; i++)
        {
            var color = dstColors[i];
            Material(color, i);
        }

        mat.Colors = dstColors;
    }

    static void Material(MtlFile.MtlColor color, int index)
    {
        static Vector4 Input(int value) => (Vector4)(Color4)Color.FromArgb(value);

        var diffuse = Input(color.Diffuse);
        var specular = Input(color.Specular);
        var specular2 = Input(color.Specular2);

        float df = index switch
        {
            WR2IDX.Blau2 => 0.5f,
            WR2IDX.DunkelRot => 0.5f, // gold
            WR2IDX.Gelb1 => 0.9f,
            WR2IDX.Gelb2 => 0.5f, // h-green
            WR2IDX.Weiss => 0.9f,
            WR2IDX.DunkelGruen2 => 0.5f, // green
            WR2IDX.Silber2 => 0.5f,
            _ => 0.8f,
        };

        float sf = index switch
        {
            WR2IDX.Gelb1 => 0.9f,
            WR2IDX.Weiss => 0.9f,
            WR2IDX.Gelb2 => 0.5f,
            WR2IDX.DunkelGruen2 => 0.5f,
            _ => 0.75f,
        };

        diffuse *= df;
        specular2 *= sf;

        color.Diffuse = ((Color4)diffuse).ToArgb();
        color.Specular = ((Color4)specular).ToArgb();
        color.Specular2 = ((Color4)specular2).ToArgb();
    }
}