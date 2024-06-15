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
    public static void Convert(string dirPath, string fileName, float ambientOffset)
    {
        var paths = new ScenarioFiles.Paths(dirPath, fileName);
        var files = new ScenarioFiles();
        files.Load(paths, GameVersion.WR1);

        ConvertQad(files.Qad);
        ConvertVtx(files.TerrainMesh.Vertices, ambientOffset);
        ConvertSni(files.Sni);

        CreateCobFiles(dirPath);

        files.Save(paths, GameVersion.WR2);
    }

    public static void ConvertSni(SniFile sni)
    {
        for (int i = 0; i < sni.Objects.Length; i++)
        {
            ref var obj = ref sni.Objects[i];
            string oldfile = obj.SoundFile;
            if (oldfile == "")
                continue;
            string newfile = $"MBWR_{oldfile}";
            obj.SoundFile = (String32)newfile;
        }
    }

    public static void ConvertQad(QadFile qad)
    {
        for (int i = 0; i < qad.Materials.Length; i++)
        {
            ConvertWR1ToWR2(ref qad.Materials[i]);
        }

        int idx0 = Array.FindIndex(qad.TextureNames, (a) => a == "Blumen");
        int idx1 = Array.FindIndex(qad.GroundPhysics, (a) => a.Name == "Gras");

        if (idx0 != -1 && idx1 != -1)
            qad.Tex2Ground[idx0] = (ushort)idx1;

        FixObjects(qad);
        FixGrounds(qad);

        qad.SetFlagsAccordingToVersion(GameVersion.WR2);
        qad.Head.FlagX2WR2 = 1;
        qad.Head.FlagX5WR2 = 1;
    }

    public static void ConvertVtx(Vertex[] vertices, float ambientOffset)
    {
        float add = ambientOffset;
        float mul = 1f - ambientOffset;

        var addvec = new Vector3(add);
        var mulvec = new Vector3(mul);

        for (int i = 0; i < vertices.Length; i++)
        {
            ref var v = ref vertices[i];

            v.LightColor = v.LightColor * mulvec + addvec;

            float temp = v.Blending.X;
            v.Blending.X = v.Blending.Y;
            v.Blending.Y = temp;
        }
    }

    public static void ConvertWR1ToWR2(ref this QadFile.AbstractMaterialType mat)
    {
        switch ((TerrainMaterialTypeMBWR)mat.Layer0.Mode)
        {
            case TerrainMaterialTypeMBWR.Terrain:
            case TerrainMaterialTypeMBWR.Terrain + 1:
            case TerrainMaterialTypeMBWR.Terrain + 2:
            {
                break;
            }
            case TerrainMaterialTypeMBWR.UVTerrain:
            {
                mat.Matrix1 = TextureTransform.Empety;
                break;
            }
            case TerrainMaterialTypeMBWR.UV:
            case TerrainMaterialTypeMBWR.UV + 1:
            {
                break;
            }
            case TerrainMaterialTypeMBWR.Road0:
            {
                mat.Layer0.Mode = TerrainMaterialTypeWR2.Road1;
                mat.Matrix1 = TextureTransform.Empety;
                mat.Matrix2 = TextureTransform.Empety;
                break;
            }
            case TerrainMaterialTypeMBWR.Reflective:
            {
                mat.Layer0.Mode = TerrainMaterialTypeWR2.Reflective;
                break;
            }
            case TerrainMaterialTypeMBWR.Road1:
            {
                mat.Layer0.Mode = TerrainMaterialTypeWR2.Road1;
                mat.Matrix0 = TextureTransform.Empety;
                mat.Matrix1 = TextureTransform.Empety;
                mat.Matrix2 = TextureTransform.Empety;
                break;
            }
            case TerrainMaterialTypeMBWR.Road3:
            {
                mat.Layer0.Mode = TerrainMaterialTypeWR2.Road3;
                mat.Matrix0 = TextureTransform.Empety;
                mat.Matrix1 = TextureTransform.Empety;
                mat.Matrix2 = TextureTransform.Empety;
                break;
            }
            case TerrainMaterialTypeMBWR.Road2:
            {
                mat.Layer0.Mode = TerrainMaterialTypeWR2.Road2;
                break;
            }
            case TerrainMaterialTypeMBWR.Water: // Water
            {
                mat.Layer0.Mode = TerrainMaterialTypeWR2.Water;
                break;
            }
            case TerrainMaterialTypeMBWR.AlphaClip: // Mask
            {
                mat.Layer0.Mode = TerrainMaterialTypeWR2.AlphaClip;
                break;
            }
            case TerrainMaterialTypeMBWR.AlphaBlend:
            {
                break;
            }
            default:
            {
                throw new InvalidDataException($"Unexpected material mode: '{mat.Layer0.Mode}'");
            }
        }
    }

    public static void FixObjects(QadFile qad)
    {
        for (int i = 0; i < qad.PropClassInfo.Length; i++)
        {
            string name = qad.PropClassObjNames[i].ToString().ToLower();
            ref var info = ref qad.PropClassInfo[i];

            var input = GetObjectInfo(name);
            if (input != null)
            {
                info.Weight = input.Weight;
                info.HitSound = (String48)input.HitSound;
                info.FallSound = (String48)input.FallSound;
            }
        }
    }

    public record ObjectInfo(int Shape, ushort Weight, string HitSound, string FallSound);
    public static ObjectInfo? GetObjectInfo(string name)
    {
        switch (name)
        {
            case "kuh":
            case "kuhl1":
            case "kuhl2":
            {
                return new ObjectInfo(0,250, "t_kuh", "ko_dirt_l");
            }
            case "pylon":
            {
                return new ObjectInfo(0,5, "ko_pylon", "ko_pylon");
            }
            case "bake1":
            case "bake2":
            {
                return new ObjectInfo(0,50, "ko_barke", "ko_barke");
            }
            case "kiste1":
            case "kiste2":
            case "hfass":
            {
                return new ObjectInfo(0,50, "ko_holz", "ko_dirt_l");
            }
            case "aleinsign":
            case "exhigh":
            {
                return new ObjectInfo(0,400, "ko_holz", "ko_dirt_l");
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
                return new ObjectInfo(0, 50, "ko_holz", "ko_dirt_l");
            }
            case "boot1":
            case "boot1a":
            case "boot1v":
            case "boot2":
            case "boot2a":
            case "boot2os":
            case "boot3":
            case "boot4":
            {
                return new ObjectInfo(4, 10000, "ko_barke2", "ko_dirt_l");
            }
            case "flugz1":
            case "flugz2":
            case "flugz3":
            case "flugz4":
            case "heli":
            case "hubi_51":
            case "hubia51":
            {
                return new ObjectInfo(4, 5000, "ko_barke2", "ko_dirt_l");
            }
            case "f15":
            case "b2bomb":
            case "aurora":
            {
                return new ObjectInfo(4, 10000, "ko_barke2", "ko_dirt_l");
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
                return new ObjectInfo(4, 600, "ko_barke2", "ko_dirt_l");
            }
        }
        return null;
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
                var info = GetObjectInfo(name.ToLower());
                if (info == null)
                {
                    continue;
                }

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
                case "grass/other":
                case "gravel":
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

        mtl.Load(mtlpath);
        mtl.Sections[0]["Diffuse"] = diffuse;
        mtl.Sections[0]["Ambient"] = ambient;
        mtl.Save(mtlpath);


        mox.Load(moxpath);
        for (int i = 0; i < mox.Head.VertCount; i++)
        {
            mox.Vertecis[i].Normal.Z = -0.25f;
        }
        mox.Save(moxpath);

    }

    public static void UpdateProps()
    {

    }
}
