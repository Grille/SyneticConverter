using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GGL.IO;
using System.Runtime.InteropServices;

namespace SyneticLib.IO.Synetic.Files;
internal class DdsFile : FileBinary
{
    public uint Magic;
    public MHeader Head;
    public Level[] Levels;
    public override void ReadFromView(BinaryViewReader br)
    {
        Head = br.Read<MHeader>();
    }

    public override void WriteToView(BinaryViewWriter bw)
    {
        bw.Write(Head);

    }

    public struct MHeader
    {
        public uint Size;
        public uint Flags;
        public uint Height;
        public uint Width;
        public uint PitchOrLinearSize;
        public uint Depth;
        public uint MipMapCount;
        Clear0Type clear0;
        public MPixelFormat ddspf;
        public uint Caps;
        public uint Caps2;
        public uint Caps3;
        public uint Caps4;
        public uint Reserved2;

        [StructLayout(LayoutKind.Sequential, Size = 44)] struct Clear0Type { }
    }

    public struct MPixelFormat
    {
        public uint Size;
        public EFlags Flags;
        public uint FourCC;
        public uint RGBBitCount;
        public uint RBitMask;
        public uint GBitMask;
        public uint BBitMask;
        public uint ABitMask;

        public enum EFlags : uint
        {
            AlphaPixels = 0x1,
            Alpha = 0x2,
            FourCC = 0x4,
            RGB = 0x40,
            YUV = 0x200,
            Luminance = 0x20000,
        }
    }

    public class Level : IViewObject
    {
        public MHead Head;
        public byte[] Decoded;
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
