using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SyneticLib.Files;

namespace SyneticLib.IO;
public static partial class Exports
{
    public static void TextureAsTga(Texture texture, string filePath)
    {
        var tga = new TgaFile();
        TextureAsTga(texture, tga);
        tga.Save(filePath);
    }

    public static void TextureAsTga(Texture texture, TgaFile tga)
    {
        var format = texture.Format switch
        {
            TextureFormat.R8 => (TgaFile.ImageType.GrayScale, 8),
            TextureFormat.BGRA32 => (TgaFile.ImageType.TrueColorImage, 32),
            _ => throw new NotImplementedException(),
        };

        tga.Head.ImageType = format.Item1;
        tga.Head.BitsPerPixel = (byte)format.Item2;
        tga.Head.Width = (ushort)texture.Width;
        tga.Head.Height = (ushort)texture.Height;
        tga.Pixels = texture.Levels[0].Data;
    }
}
