using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Drawing.Drawing2D;
using OpenTK.Mathematics;
using SyneticLib.Files;



namespace SyneticLib.Conversion;

public static class WR1ToWR2FileConv
{
    public static void Convert(string path, float ambientOffset)
    {
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

        for (int i = 0; i < sni.Objects.Length; i++)
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
        var qad = new QadFile();
        qad.SetFlagsAccordingToVersion(GameVersion.WR1);
        qad.Load(path);

        for (int i = 0; i < qad.MaterialsWR.Length; i++)
        {
            ConvertWR1ToWR2(ref qad.MaterialsWR[i]);
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
        qad.Head.FlagX2WR2 = 1;
        qad.Head.FlagX5WR2 = 1;
        qad.Save();

    }

    public static void ConvertVtx(string path, float ambientOffset)
    {
        var vtx = new VtxFile();
        vtx.Load(path);

        float add = ambientOffset;
        float mul = 1f - ambientOffset;

        var addvec = new Vector3(add);
        var mulvec = new Vector3(mul);

        for (int i = 0; i < vtx.Vertecis.Length; i++)
        {
            ref var v = ref vtx.Vertecis[i];

            v.LightColor = v.LightColor * mulvec + addvec;

            float temp = v.Blending.X;
            v.Blending.X = v.Blending.Y;
            v.Blending.Y = temp;
        }

        vtx.Save(path);
    }

    public static void ConvertWR1ToWR2(ref this QadFile.MMaterialTypeWR mat)
    {
        switch ((TerrainMaterialTypeMBWR)mat.Mode)
        {
            case TerrainMaterialTypeMBWR.Terrain:
            case TerrainMaterialTypeMBWR.Terrain + 1:
            case TerrainMaterialTypeMBWR.Terrain + 2:
            {
                break;
            }
            case TerrainMaterialTypeMBWR.UVTerrain:
            {
                mat.Matrix1 = Transform.Empety;
                break;
            }
            case TerrainMaterialTypeMBWR.UV:
            case TerrainMaterialTypeMBWR.UV + 1:
            {
                break;
            }
            case TerrainMaterialTypeMBWR.Road0:
            {
                mat.Mode = TerrainMaterialTypeWR2.Road1;
                mat.Matrix1 = Transform.Empety;
                mat.Matrix2 = Transform.Empety;
                break;
            }
            case TerrainMaterialTypeMBWR.Reflective:
            {
                mat.Mode = TerrainMaterialTypeWR2.Reflective;
                break;
            }
            case TerrainMaterialTypeMBWR.Road1:
            {
                mat.Mode = TerrainMaterialTypeWR2.Road1;
                mat.Matrix0 = Transform.Empety;
                mat.Matrix1 = Transform.Empety;
                mat.Matrix2 = Transform.Empety;
                break;
            }
            case TerrainMaterialTypeMBWR.Road3:
            {
                mat.Mode = TerrainMaterialTypeWR2.Road3;
                mat.Matrix0 = Transform.Empety;
                mat.Matrix1 = Transform.Empety;
                mat.Matrix2 = Transform.Empety;
                break;
            }
            case TerrainMaterialTypeMBWR.Road2:
            {
                mat.Mode = TerrainMaterialTypeWR2.Road2;
                break;
            }
            case TerrainMaterialTypeMBWR.Water: // Water
            {
                mat.Mode = TerrainMaterialTypeWR2.Water;
                mat.MatrixChecksum0 = 61420826;
                mat.MatrixChecksum1 = 75580286;
                mat.MatrixChecksum2 = 78329526;
                break;
            }
            case TerrainMaterialTypeMBWR.AlphaClip: // Mask
            {
                mat.Mode = TerrainMaterialTypeWR2.AlphaClip;
                break;
            }
            case TerrainMaterialTypeMBWR.AlphaBlend:
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
        for (int i = 0; i < qad.GroundPhysics.Length; i++)
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
        mtl.Sections[0]["Diffuse"] = diffuse;
        mtl.Sections[0]["Ambient"] = ambient;
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
}
