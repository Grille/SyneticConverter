using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;
using System.Drawing;
using OpenTK.Mathematics;

namespace SyneticLib.Utils;

public static class ConvertMbwrCar
{
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

        var dstColors = new MtlFile.MtlColor[srcColors.Length];

        dstColors[0] = srcColors[0]; //b
        dstColors[1] = srcColors[1]; //r0
        dstColors[2] = srcColors[2]; //r1
        dstColors[3] = srcColors[4]; //y->go
        dstColors[4] = srcColors[3]; //go->y
        dstColors[5] = srcColors[13]; //w->hg
        dstColors[6] = srcColors[5]; //gray->white
        dstColors[7] = srcColors[6]; //dgreen->gray
        dstColors[8] = srcColors[7]; //dblue->dgreen
        dstColors[9] = srcColors[11]; //blue->green
        dstColors[10] = srcColors[9]; //lblue -> blue
        dstColors[11] = srcColors[10]; //green -> lblue
        dstColors[12] = srcColors[8]; //v->dblue
        dstColors[13] = srcColors[12]; //hg->silver
        dstColors[14] = srcColors[14];

        for (int i = 0; i < dstColors.Length; i++)
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
            3 => 0.5f, // gold
            5 => 0.5f, // h-green
            6 => 0.9f,
            9 => 0.5f, // green
            14 => 0.5f,
            _ => 0.8f,
        };

        float sf = index switch
        {
            5 => 0.5f,
            9 => 0.5f,
            _ => 0.75f,
        };

        diffuse *= df;
        specular2 *= sf;

        color.Diffuse = ((Color4)diffuse).ToArgb();
        color.Specular = ((Color4)specular).ToArgb();
        color.Specular2 = ((Color4)specular2).ToArgb();
    }
}
