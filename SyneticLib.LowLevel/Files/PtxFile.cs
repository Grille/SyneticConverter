using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Grille.IO;
using SyneticLib.LowLevel.Compression;
using SyneticLib.Files.Common;

namespace SyneticLib.Files;

public class PtxFile : BinaryFile, ITextureData<PtxFile.Level>
{
    public MHead Head;
    public Level[] Levels;

    public PtxFile()
    {
        Levels = Array.Empty<Level>();
    }

    public override void Deserialize(BinaryViewReader br)
    {
        Head = br.Read<MHead>();

        if (Head.Clear0 != 0)
            throw new InvalidDataException();

        Levels = new Level[Head.MipMapLevels];
        for (int i = 0; i < Levels.Length; i++)
        {
            var level = new Level();
            level.ReadFromView(br);
            Levels[i] = level;
        }
    }

    public override void Serialize(BinaryViewWriter bw)
    {
        bw.Write(Head);
        for (int i = 0; i < Levels.Length; i++)
        {
            Levels[i].WriteToView(bw);
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

    public class Level : ITextureDataLevel
    {
        public MHead Head;
        public byte[] Pixels;

        public Level()
        {
            Pixels = Array.Empty<byte>();
        }

        public Level(byte[] pixels)
        {
            Pixels = pixels;
            Head.Size = (uint)Pixels.Length;
            Head.SynSize = 0;
        }

        public void ReadFromView(BinaryViewReader br)
        {
            Head = br.Read<MHead>();

            if (Head.SynSize > 0)
            {
                //br.PeakStream.Seek(Head.SynSize, SeekOrigin.Current);
                //Pixels = new byte[Head.Size];

                
                var DecodeStream = new MemoryStream();
                SynCompressor.Decompress(br.PeakStream, DecodeStream, (int)Head.SynSize);
                Pixels = DecodeStream.ToArray();
                long diff = Pixels.Length - Head.Size;
                if (diff != 0)
                {
                    throw new InvalidDataException($"Invalid size (result:{Pixels.Length} != expected:{Head.Size}) diff:{diff}");
                }
                

            }
            else
            {
                Pixels = br.ReadArray<byte>(Head.Size);
            }
        }

        public void WriteToView(BinaryViewWriter bw)
        {
            bw.Write(Head);
            bw.WriteArray(Pixels, LengthPrefix.None);
        }

        public struct MHead
        {
            public uint Size, SynSize;
        }
    }
}
