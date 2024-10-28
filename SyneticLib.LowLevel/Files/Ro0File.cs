using System.IO;

using Grille.IO;

using OpenTK.Mathematics;

using SyneticLib.Files.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SyneticLib.Files;

public class Ro0File : BinaryFile
{
    public MHead Head;
    public String32 TextureName;
    public MUV[] UV;
    public MChunk[] Chunks;
    public GrassGeneric[] Grass;

    public GameVersion Version;

    public Ro0File()
    {
        UV = Array.Empty<MUV>();
        Chunks = Array.Empty<MChunk>();
        Grass = Array.Empty<GrassGeneric>();
    }

    public override void Deserialize(BinaryViewReader br)
    {
        Head = br.Read<MHead>();
        TextureName = br.Read<String32>();

        UV = br.ReadArray<MUV>(Head.Variants);
        Chunks = br.ReadArray<MChunk>(Head.ChunksLength);

        Grass = new GrassGeneric[Head.GrassLength];
        switch (Head.X1)
        {
            case 0:
            {
                for (int i = 0; i < Grass.Length; i++)
                {
                    Grass[i] = br.Read<MGrassWR2>();
                }
                break;
            }
            case 2:
            {
                for (int i = 0; i < Grass.Length; i++)
                {
                    Grass[i] = br.Read<MGrassCT1>();
                }
                break;
            }
            default:
            {
                throw new InvalidDataException();
            }
        }
    }

    public override void Serialize(BinaryViewWriter bw)
    {
        bw.Write(Head);
        bw.Write(TextureName);

        bw.WriteArray(UV, LengthPrefix.None);
        bw.WriteArray(Chunks, LengthPrefix.None);

        switch (Head.X1)
        {
            case 0:
            {
                for (int i = 0; i < Grass.Length; i++)
                {
                    bw.Write((MGrassWR2)Grass[i]);
                }
                break;
            }
            case 2:
            {
                for (int i = 0; i < Grass.Length; i++)
                {
                    bw.Write((MGrassCT1)Grass[i]);
                }
                break;
            }
            default:
            {
                throw new InvalidDataException();
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MUV
    {
        public Vector2 A, B, C, D;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MHead
    {
        public ushort X1, X2, X3, X4;
        public int Variants;
        public int ChunksWidth;
        public int ChunksHeight;
        public int ChunksLength;
        public int GrassLength;
        public int Density;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MChunk
    {
        public int Begin;
        public int Length;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MGrassWR2
    {
        public Vector3 Position;
        public byte ID;
        public byte Size;
        public ColorU16R4G4B4 RgbaColor;

        public static implicit operator GrassGeneric(MGrassWR2 a) => new()
        {
            Position = a.Position,
            ID = a.ID,
            Size = a.Size,
            Color0 = a.RgbaColor.Decode(),
        };

        public static explicit operator MGrassWR2(GrassGeneric a) => new()
        {
            Position = a.Position,
            ID = a.ID,
            Size = a.Size,
            RgbaColor = new ColorU16R4G4B4(a.Color0),
        };
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MGrassCT1
    {
        public Vector3 Position;
        public byte ID;
        public byte Size;
        public ColorU16R6G5B5 Color0;
        public ColorU16R6G5B5 Color1;

        public static implicit operator GrassGeneric(MGrassCT1 a) => new()
        {
            Position = a.Position,
            ID = a.ID,
            Size = a.Size,
            Color0 = a.Color0.Decode(),
            Color1 = a.Color1.Decode(),
        };

        public static explicit operator MGrassCT1(GrassGeneric a) => new()
        {
            Position = a.Position,
            ID = a.ID,
            Size = a.Size,
            Color0 = new ColorU16R6G5B5(a.Color0),
            Color1 = new ColorU16R6G5B5(a.Color1),
        };
    }

    public struct GrassGeneric
    {
        public Vector3 Position;
        public byte ID;
        public byte Size;
        public Vector3 Color0;
        public Vector3 Color1;
    }

    public struct ColorU16R4G4B4
    {
        public ushort Value;

        public ColorU16R4G4B4(Vector3 rgb)
        {
            Encode(rgb);
        }

        public Vector3 Decode()
        {
            static float Decode(ushort value, int shift) => ((value >> shift) & 15) / 15f;

            return new Vector3(Decode(Value, 8), Decode(Value, 4), Decode(Value, 0));
        }

        public void Encode(Vector3 rgb)
        {
            Encode(rgb.X, rgb.Y, rgb.Z);
        }

        public void Encode(float r, float g, float b)
        {
            static int Encode(float value) => (int)(value * 15f + 0.5f);

            Value = (ushort)(Encode(r) << 8 | Encode(g) << 4 | Encode(b) << 0);
        }
    }

    public struct ColorU16R6G5B5
    {
        public ushort Value;

        public ColorU16R6G5B5(Vector3 rgb)
        {
            Encode(rgb);
        }

        public Vector3 Decode()
        {
            float r = ((Value >> 10) & 63) / 63f;
            float g = ((Value >> 5) & 31) / 31f;
            float b = ((Value >> 0) & 31) / 31f;

            return new Vector3(r, g, b);
        }

        public void Encode(Vector3 rgb)
        {
            Encode(rgb.X, rgb.Y, rgb.Z);
        }

        public void Encode(float r, float g, float b)
        {
            static int Encode(float value, int mul) => (int)(value * mul + 0.5f);
            Value = (ushort)(Encode(r, 63) << 10 | Encode(g, 31) << 5 | Encode(b, 31) << 0);
        }
    }
}
