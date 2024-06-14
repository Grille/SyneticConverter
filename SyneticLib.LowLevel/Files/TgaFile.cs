using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.IO;

using SyneticLib.Files.Common;
using System.Runtime.InteropServices;

namespace SyneticLib.Files;

public class TgaFile : BinaryFile
{
    public MHead Head;

    public byte[] Pixels;

    public TgaFile()
    {
        Pixels = Array.Empty<byte>();
    }

    public override void Deserialize(BinaryViewReader br)
    {
        Head = br.Read<MHead>();
        if (Head.IdLength > 0)
        {
            br.Seek(Head.IdLength, SeekOrigin.Current);
        }
        int size = Head.Width * Head.Height * (Head.BitsPerPixel / 8);
        Pixels = br.ReadArray<byte>(size);
    }

    public override void Serialize(BinaryViewWriter bw)
    {
        bw.Write(Head);
        bw.WriteArray(Pixels, LengthPrefix.None);
    }

    public enum ImageType : byte
    {
        None = 0,
        TrueColorImage = 2,
        GrayScale = 3,
    }

    [Flags]
    public enum ImageDescriptor : byte
    {
        ScreenOrigin = 0b00001000
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MHead
    {
        public byte IdLength;
        public byte ColorMapType;
        public ImageType ImageType;
        public ushort ColorMapIndex;
        public ushort ColorMapLength;
        public byte ColorMapSize;
        public ushort OriginX;
        public ushort OriginY;
        public ushort Width;
        public ushort Height;
        public byte BitsPerPixel;
        public ImageDescriptor ImageDescriptor;
    }
}
