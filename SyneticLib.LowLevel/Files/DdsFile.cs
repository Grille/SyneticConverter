using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Grille.IO;
using System.Runtime.InteropServices;
using SyneticLib.LowLevel.Compression;
using SyneticLib.Files.Common;

namespace SyneticLib.Files;
public unsafe class DdsFile : BinaryFile
{
    public const uint MagicValue = 0x20534444;

    public uint Magic;
    public MHeader Head;
    public Level[] Levels;

    public DdsFile()
    {
        Levels = Array.Empty<Level>();
    }

    public override void Deserialize(BinaryViewReader br)
    {
        Magic = br.ReadUInt32();
        if (Magic != MagicValue)
            throw new InvalidDataException();

        Head = br.Read<MHeader>();
        Head.Assert();
    }

    public override void Serialize(BinaryViewWriter bw)
    {
        bw.Write(Head);

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MHeader
    {
        public uint Size;
        public MHeaderFlags Flags;
        public uint Height;
        public uint Width;
        public uint PitchOrLinearSize;
        public uint Depth;
        public uint MipMapCount;
        public fixed uint Reserved1[11];
        public MPixelFormat PixelFormat;
        public MHeaderCaps1 Caps1;
        public uint Caps2;
        public uint Caps3;
        public uint Caps4;
        public uint Reserved2;

        public void Assert()
        {
            if (Size != 124)
                throw new InvalidDataException();
        }
    }

    [Flags]
    public enum MHeaderFlags : uint
    {
        Caps = 0x1,
        Height = 0x2,
        Width = 0x4,
        Pitch = 0x8,
        PixelFormat = 0x1000,
        MipmapCount = 0x20000,
        LinearSize = 0x80000,
        Depth = 0x800000,
    }

    [Flags]
    public enum MHeaderCaps1 : uint
    {
        Complex = 0x8,
        Mipmap = 0x400000,
        Texture = 0x1000,
    }

    public struct MPixelFormat
    {
        public uint Size;
        public MPixelFormatFlags Flags;
        public uint FourCC;
        public uint RGBBitCount;
        public uint RBitMask;
        public uint GBitMask;
        public uint BBitMask;
        public uint ABitMask;
    }

    [Flags]
    public enum MPixelFormatFlags : uint
    {
        AlphaPixels = 0x1,
        Alpha = 0x2,
        FourCC = 0x4,
        RGB = 0x40,
        YUV = 0x200,
        Luminance = 0x20000,
    }

    public class Level
    {
        public MHead Head;
        public byte[] Decoded;

        public Level()
        {
            Decoded = Array.Empty<byte>();
        }

        public void ReadFromView(BinaryViewReader br)
        {
            Head = br.Read<MHead>();

            if (Head.SynSize > 0)
            {
                var DecodeStream = new MemoryStream();
                SynCompressor.Decompress(br.PeakStream, DecodeStream, (int)Head.SynSize);
                Decoded = DecodeStream.ToArray();
                long diff = Decoded.Length - Head.Size;
                if (diff != 0)
                {
                    throw new InvalidDataException($"Invalid size (result:{Decoded.Length} != expected:{Head.Size}) diff:{diff}");
                }

            }
            else
            {
                Decoded = br.ReadArray<byte>(Head.Size);
            }
        }

        public void WriteToView(BinaryViewWriter bw)
        {
            Head.Size = (uint)Decoded.Length;
            Head.SynSize = 0;

            bw.Write(Head);
            bw.WriteArray(Decoded, LengthPrefix.None);
        }

        public struct MHead
        {
            public uint Size, SynSize;
        }
    }
}
