using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GGL.IO;
using SyneticLib.LowLevel.Compression;

namespace SyneticLib.LowLevel.Files;

public class PtxFile : BinaryFile
{
    public MHead Head;
    public Level[] Levels;
    public override void ReadFromView(BinaryViewReader br)
    {
        Head = br.Read<MHead>();

        if (Head.Clear0 != 0)
            throw new InvalidDataException();

        Levels = new Level[Head.MipMapLevels];
        for (int i = 0; i < Levels.Length; i++)
        {
            Levels[i] = br.ReadIView<Level>();
        }
    }

    public override void WriteToView(BinaryViewWriter bw)
    {
        bw.Write(Head);
        for (int i = 0; i < Levels.Length; i++)
        {
            bw.WriteIView(Levels[i]);
        }
    }

    public struct MHead
    {
        public byte Compression;
        public byte BitPerPixel;
        public ushort Clear0;
        public int Width;
        public int Height;
        public byte MipMapLevels;
        public byte R, G, B;
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
