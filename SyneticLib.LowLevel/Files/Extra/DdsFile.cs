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
using static SyneticLib.Files.QadFile;

namespace SyneticLib.Files.Extra;
public unsafe class DdsFile : BinaryFile, ITextureData<DdsFile.Level>
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

        int mipMapCount = Head.MipMapCount;

        int currentWidth = Head.Width;
        int currentHeight = Head.Height;

        int blockSize = Head.PixelFormat.FourCC switch
        {
            MFourCharCode.DXT1 => 8,
            MFourCharCode.DXT5 => 16,
            MFourCharCode.None => 0,
            _ => throw new NotSupportedException(Head.PixelFormat.FourCC.ToString()),
        };

        Levels = new Level[mipMapCount];

        var size = (int)Head.PixelFormat.Size / 8;

        for (int i = 0; i < mipMapCount; i++)
        {
            if (blockSize != 0)
            {
                Levels[i] = ReadCompressedLevel(br, currentWidth, currentHeight, blockSize);
            }
            else
            {
                Levels[i] = ReadLevel(br, currentWidth, currentHeight, size);
            }

            currentWidth /= 2;
            currentHeight /= 2; 
        }
    }

    private Level ReadCompressedLevel(BinaryViewReader br, int width, int height, int blockSize)
    {
        int levelSize = Math.Max(1, (width + 3) / 4) * Math.Max(1, (height + 3) / 4) * blockSize;

        var data = br.ReadArray<byte>(levelSize);

        return new Level(data, width, height);
    }

    private Level ReadLevel(BinaryViewReader br, int width, int height, int size)
    {
        int levelSize = width * height * size;

        var data = br.ReadArray<byte>(levelSize);

        return new Level(data, width, height);
    }

    public override void Serialize(BinaryViewWriter bw)
    {
        bw.Write(Magic);
        bw.Write(Head);

        foreach (var level in Levels)
        {
            bw.WriteArray(level.Data, LengthPrefix.None);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MHeader
    {
        public int Size;
        public MHeaderFlags Flags;
        public int Height;
        public int Width;
        public int PitchOrLinearSize;
        public int Depth;
        public int MipMapCount;
        public fixed int Reserved1[11];
        public MPixelFormat PixelFormat;
        public MHeaderCaps1 Caps1;
        public int Caps2;
        public int Caps3;
        public int Caps4;
        public int Reserved2;

        public void Assert()
        {
            if (PixelFormat.FourCC == MFourCharCode.DX10)
            {
                throw new InvalidDataException("DX10 extension not supported.");
            }

            if (Size != 124)
            {
                throw new InvalidDataException();
            }
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
        public String4 FourCCString;
        public uint RGBBitCount;
        public uint RBitMask;
        public uint GBitMask;
        public uint BBitMask;
        public uint ABitMask;

        public MFourCharCode FourCC
        {
            set => FourCCString = (String4)value.ToString();
            get
            {
                var result = Enum.TryParse<MFourCharCode>(FourCCString, true, out var code);
                return result ? code : 0;
            }
        }
    }

    public enum MFourCharCode
    {
        None,
        DXT1,
        DXT2,
        DXT3,
        DXT4,
        DXT5,
        DX10,
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

    public class Level : ITextureDataLevel
    {
        public int Width;
        public int Height;
        public byte[] Data;

        public Level(byte[] data, int width, int height)
        {
            Data = data;
            Width = width;
            Height = height;
        }
    }
}
