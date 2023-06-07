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

namespace SyneticPipelineTool;

public static class WR1ToWR2Conv
{
    public static void ConvertVGroup(string path)
    {
        string name = Path.GetFileName(path);
        string v1 = Path.Join(path, "V1");
        string v2 = Path.Join(path, "V2");
        string v3 = Path.Join(path, "V3");
        /*
        ConvertV(v1, name);
        ConvertV(v2, name);
        ConvertV(v3, name);
        */
    }

    public static void Convert(string path, float ambientOffset)
    {
        Console.WriteLine($"Convert '{path}'");

        string qadpath = Path.ChangeExtension(path, ".qad");
        string vtxpath = Path.ChangeExtension(path, ".vtx");
        string snipath = Path.ChangeExtension(path, ".sni");

        ConvertQad(qadpath);
        ConvertVtx(vtxpath, ambientOffset);
        ConvertSni(snipath);
    }

    public static void ConvertSni(string path)
    {
        var sni = new SniFile();
        sni.Load(path);

        for (int i = 0; i< sni.Objects.Length; i++)
        {
            ref var obj = ref sni.Objects[i];
            string oldfile = obj.SoundFile;
            if (oldfile == "")
                continue;
            string newfile = $"MBWR_{oldfile}";
            obj.SoundFile = (String32)newfile;
        }

        sni.Save();
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

        int idx0 = Array.FindIndex(qad.TextureNames, (a) => a == "Blumen");
        int idx1 = Array.FindIndex(qad.GroundPhysics, (a) => a.Name == "Gras");

        if (idx0 != -1 && idx1 != -1)
            qad.Tex2Ground[idx0] = (ushort)idx1;

        FixObjects(qad);
        FixGrounds(qad);

        qad.SortMaterials();
        qad.RecalcMaterialChecksum();

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

    public static void FixObjects(QadFile qad)
    {
        for (int i = 0; i < qad.PropClassInfo.Length; i++)
        {
            string name = qad.PropClassObjNames[i];
            ref var info = ref qad.PropClassInfo[i];

            switch (name.ToLower())
            {
                case "kuh":
                case "kuhl1":
                case "kuhl2":
                {
                    info.Shape = 4;
                    info.Weight = 250;
                    info.HitSound = (String48)"t_kuh";
                    info.FallSound = (String48)"ko_dirt_l";
                    break;
                }
                case "pylon":
                {
                    info.Shape = 1;
                    info.Mode = 0;
                    info.Weight = 5;
                    info.HitSound = (String48)"ko_pylon";
                    info.FallSound = (String48)"ko_pylon";
                    break;
                }
                case "bake1":
                case "bake2":
                {
                    info.Shape = 2;
                    info.Mode = 0;
                    info.Weight = 50;
                    info.HitSound = (String48)"ko_barke";
                    info.FallSound = (String48)"ko_barke";
                    break;
                }
                case "tunnlampe":
                {
                    info.Mode = 0;
                    break;
                }
                case "hfass":
                {
                    info.Shape = 4;
                    info.Weight = 50;
                    info.HitSound = (String48)"ko_holz";
                    info.FallSound = (String48)"ko_dirt_l";
                    break;
                }
                case "sign1":
                case "sign2":
                case "sign3":
                case "sign4":
                case "sign5":
                case "sign6":
                case "sign7":
                case "sign8":
                case "sign9":
                case "sign10":
                case "sign11":
                case "sign12":
                case "warns1":
                case "warns2":
                {
                    info.Shape = 4;
                    info.Weight = 50;
                    info.HitSound = (String48)"ko_holz";
                    info.FallSound = (String48)"ko_dirt_l";
                    break;
                }
                case "hubi_51":
                case "hubia51":
                {
                    break;
                }
                case "f15":
                case "b2bomb":
                case "aurora":
                {
                    break;
                }
                case "car1":
                case "car2":
                case "car3":
                case "car4":
                case "car5":
                case "car6":
                case "car7":
                case "car8":
                case "van1":
                case "gland1":
                case "gland2":
                case "trackvan":
                {
                    info.Shape = 3;
                    info.Weight = 600;
                    info.HitSound = (String48)"ko_barke2";
                    info.FallSound = (String48)"ko_dirt_l";
                    break;
                }

            }
        }
    }

    public static void FixGrounds(QadFile qad)
    {
        for (int i = 0; i< qad.GroundPhysics.Length; i++)
        {
            ref var pys = ref qad.GroundPhysics[i];

            switch (((string)pys.Name).ToLower())
            {
                case "kies":
                case "kiesbett":
                case "gras":
                case "grass":
                case "sand":
                case "erde-sand":
                case "erdemittel":
                case "erdestark":
                case "erdeasph":
                case "feldweg":
                case "schmutz":
                {
                    pys.SkidID = 2;
                    pys.x8 = 2;
                    break;
                }
                case "schnee":
                {
                    pys.NoiseID = 1;
                    pys.SkidID = 2;
                    pys.x8 = 2;
                    break;
                }
                case "curbs":
                case "beton":
                case "strasselstaub":
                {
                    pys.NoiseID = 0;
                    pys.SkidID = 1;
                    break;
                }
            }
        }
    }

    public static void FixTreeSprite(string path, bool ignoreMissing, string diffuse = "0xa0a0a0", string ambient = "0x1a1a1a")
    {
        string moxpath = Path.ChangeExtension(path, "mox");
        string mtlpath = Path.ChangeExtension(path, "mtl");

        if (!File.Exists(moxpath) && !File.Exists(mtlpath) && ignoreMissing)
            return;

        var mox = new MoxFile();
        var mtl = new MtlFile();
        mox.Path = moxpath;
        mtl.Path = mtlpath;

        mtl.Load();
        mtl.Materials[0]["Diffuse"] = new[] { diffuse };
        mtl.Materials[0]["Ambient"] = new[] { ambient };
        mtl.Save();

        
        mox.Load();
        for (int i = 0; i < mox.Head.VertCount; i++)
        {
            mox.Vertecis[i].Normal.Z = -0.25f;
        }
        mox.Save();
        
    }

    public static void UpdateProps()
    {

    }

    public static void ConvertVtx(string path, float ambientOffset)
    {
        Console.WriteLine($"Convert Vtx '{path}'");
        var vtx = new VtxFile();
        vtx.Load(path);

        float add = 255 * ambientOffset;
        float mul = 1f - ambientOffset;
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
