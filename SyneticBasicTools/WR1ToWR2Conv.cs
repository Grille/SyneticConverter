using SyneticLib;
using SyneticLib.IO.Synetic.Files;
using System;
using System.IO;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Drawing.Drawing2D;

namespace SyneticBasicTools;

public static class WR1ToWR2Conv
{
    public static void ConvertVGroup(string path)
    {
        string name = Path.GetFileName(path);
        string v1 = Path.Join(path, "V1");
        string v2 = Path.Join(path, "V2");
        string v3 = Path.Join(path, "V3");

        ConvertV(v1, name);
        ConvertV(v2, name);
        ConvertV(v3, name);
    }

    public static void ConvertV(string path, string name)
    {
        Console.WriteLine($"Convert '{path}'");
        ConvertQad(Path.Join(path, name + ".qad"));
        ConvertVtx(Path.Join(path, name + ".vtx"));
    }

    public static void ConvertQad(string path)
    {
        Console.WriteLine($"Convert Qad '{path}'");

        var qad = new QadFile();
        qad.SetFlagsAccordingToVersion(GameVersion.WR1);
        qad.Load(path);

        for (int i = 0; i < qad.Materials.Length; i++)
        {
            ConvertMaterial(ref qad.Materials[i]);
        }

        for (int i = 0; i < qad.PropClassInfo.Length; i++)
        {
            ref var prop = ref qad.PropClassInfo[i];
        }

        for (int i = 0; i < qad.Chunks.Length; i++)
        {
            ref var chunk = ref qad.Chunks[i];
        }


        qad.SetFlagsAccordingToVersion(GameVersion.WR2);
        qad.Head.FlagX2 = 1;
        qad.Head.FlagX5 = 1;
        qad.Save();
        
    }

    public static void ConvertMaterial(ref QadFile.MMaterialType1 mat)
    {
        switch (mat.Mode)
        {
            case 0:
            case 1:
            case 2:
            {
                break;
            }
            case 16:
            {
                mat.Matrix1 = Transform.Empety;
                break;
            }
            case 32:
            case 33:
            {
                break;
            }
            case 48:
            {
                mat.Mode = 144;
                mat.Matrix1 = Transform.Empety;
                mat.Matrix2 = Transform.Empety;
                break;
            }
            case 64: // Reflective
            {
                mat.Mode = 112;
                break;
            }
            case 80:
            {
                mat.Mode = 144;
                mat.Matrix0 = Transform.Empety;
                mat.Matrix1 = Transform.Empety;
                mat.Matrix2 = Transform.Empety;
                break;
            }
            case 96:
            {
                mat.Mode = 176;
                mat.Matrix0 = Transform.Empety;
                mat.Matrix1 = Transform.Empety;
                mat.Matrix2 = Transform.Empety;
                break;
            }
            case 112:
            {
                mat.Mode = 160;
                break;
            }
            case 208: // Water
            {
                mat.Mode = 224;
                mat.X0 = 61420826;
                mat.X1 = 75580286;
                mat.X2 = 78329526;
                break;
            }
            case 224: // Mask
            {
                mat.Mode = 208;
                break;
            }
            case 240:
            {
                break;
            }
            default:
            {
                throw new InvalidDataException($"Unexpected material mode: '{mat.Mode}'");
            }
        }
    }

    public static void ConvertVtx(string path)
    {
        Console.WriteLine($"Convert Vtx '{path}'");
        var vtx = new VtxFile();
        vtx.Load(path);

        float lightScale = 0.25f;
        float add = 255 * lightScale;
        float mul = 1f - lightScale;
        foreach (var v in vtx.Vertecis)
        {
            v.LightColor.R = (byte)(v.LightColor.R * mul + add);
            v.LightColor.G = (byte)(v.LightColor.G * mul + add);
            v.LightColor.B = (byte)(v.LightColor.B * mul + add);

            float temp = v.Blending.X;
            v.Blending.X = v.Blending.Y;
            v.Blending.Y = temp;
        }

        vtx.Save(path);
    }
}
