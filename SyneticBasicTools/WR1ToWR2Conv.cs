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

namespace SyneticBasicTools;

public class WR1ToWR2Conv
{
    GameDirectory MBWR;
    GameDirectory WR2;

    public WR1ToWR2Conv(string srcDirectory, string dstDirectory)
    {
        MBWR = new GameDirectory(srcDirectory, GameVersion.MBWR);
        WR2 = new GameDirectory(dstDirectory, GameVersion.WR2);

        MBWR.Scenarios.Seek();
        MBWR.Sounds.Seek();
    }

    public void CopySounds()
    {
        MBWR.Sounds.CopyFilesWithPrefixTo(WR2.Sounds.SourcePath,"MBWR_");
    }

    public void ConvertVGroup(string name)
    {
        string scn = Path.Join(WR2.SourcePath, "Scenarios", name);
        string v1 = Path.Join(scn, "V1");
        string v2 = Path.Join(scn, "V2");
        string v3 = Path.Join(scn, "V3");

        ConvertV(v1, name);
        ConvertV(v2, name);
        ConvertV(v3, name);
    }

    public void ConvertV(string path, string name)
    {
        Console.WriteLine($"Convert '{path}'");
        ConvertQad(Path.Join(path, name + ".qad"));
        ConvertVtx(Path.Join(path, name + ".vtx"));
    }

    public void ConvertQad(string path)
    {
        Console.WriteLine($"Convert Qad '{path}'");

        var qad = new QadFile();
        qad.SetFlagsAccordingToVersion(GameVersion.MBWR);
        qad.Load(path);

        qad.Head.FlagX2 = 1;
        qad.Head.FlagX5 = 1;

        var matrix = Transform.Empety;

        
        for (int i = 0; i< qad.Materials.Length; i++)
        {
            ref var mat = ref qad.Materials[i];
            switch (mat.Mode)
            {
                case 2:
                {
                    mat.Mode = 0;
                    break;
                }
                case 16:
                {
                    mat.Matrix1 = matrix;
                    break;
                }
                case 48:
                {
                    mat.Mode = 144;
                    mat.Matrix1 = matrix;
                    mat.Matrix2 = matrix;
                    break;
                }
                case 80:
                {
                    mat.Mode = 144;
                    mat.Matrix0 = matrix;
                    mat.Matrix1 = matrix;
                    mat.Matrix2 = matrix;
                    break;
                }
                case 112:
                {
                    mat.Mode = 160;
                    break;
                }
                case 96:
                {
                    mat.Mode = 176;
                    mat.Matrix0 = matrix;
                    mat.Matrix1 = matrix;
                    mat.Matrix2 = matrix;
                    break;
                }
                case 64: // Reflective
                {
                    mat.Mode = 112;
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

            }
        }

        for (int i = 0; i < qad.PropClassInfo.Length; i++)
        {
            ref var prop = ref qad.PropClassInfo[i];
        }

        for (int i = 0; i < qad.Chunks.Length; i++)
        {
            ref var chunk = ref qad.Chunks[i];
            chunk.Radius *= 2;
        }


        qad.SetFlagsAccordingToVersion(GameVersion.WR2);
        qad.Save();
        
    }

    public void ConvertVtx(string path)
    {
        Console.WriteLine($"Convert Vtx '{path}'");
        var vtx = new VtxFile();
        vtx.Load(path);

        foreach (var v in vtx.Vertecis)
        {
            v.LightColor.R = (byte)(v.LightColor.R * 0.75 + 64);
            v.LightColor.G = (byte)(v.LightColor.G * 0.75 + 64);
            v.LightColor.B = (byte)(v.LightColor.B * 0.75 + 64);

            float temp = v.Blending.X;
            v.Blending.X = v.Blending.Y;
            v.Blending.Y = temp;
            
            //v.LightColor.A = (byte)(v.LightColor.A / 2 + 32);
        }

        vtx.Save(path);
    }

    public void Log(string name)
    {
        string scn = Path.Join(WR2.SourcePath, "Scenarios", name);
        string v1 = Path.Join(scn, "V1");
        string path = Path.Join(v1, name + ".qad");
        Console.WriteLine(path);
        var qad = new QadFile();
        qad.Load(path);

        foreach (var m in qad.Materials)
        {
            if (m.Mode == 224)
            {
                Console.WriteLine("----");
                Console.WriteLine(m.X0);
                Console.WriteLine(m.X1);
                Console.WriteLine(m.X2);
            }
        }
    }



    public void CopyScn()
    {

    }
}
