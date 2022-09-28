using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GGL.IO;

namespace SyneticLib.IO.Synetic.Files;
internal class PtxFile : SyneticBinaryFile
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
        public byte[] Encoded;
        public byte[] Decoded;
        public void ReadFromView(BinaryViewReader br)
        {
            Head = br.Read<MHead>();

            if (Head.SynSize > 0)
            {
                Encoded = br.ReadArray<byte>(Head.SynSize);
                Decoded = new byte[Head.Size];

                SynCompressor.Decompress(Encoded, Decoded);
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
