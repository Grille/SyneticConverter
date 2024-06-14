using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.Files;

namespace SyneticLib.IO;
public static partial class Imports
{
    public static Texture LoadTgaTexture(string path)
    {
        var tga = new TgaFile();
        tga.Load(path);
        return LoadTgaTexture(tga);
    }
    public static Texture LoadTgaTexture(TgaFile tga)
    {
        var format = (tga.Head.ImageType, tga.Head.BitsPerPixel) switch
        {
            (TgaFile.ImageType.GrayScale, 8) => TextureFormat.R8,
            (TgaFile.ImageType.TrueColorImage, 8) => TextureFormat.R8,
            (TgaFile.ImageType.TrueColorImage, 24) => TextureFormat.RGB24,
            (TgaFile.ImageType.TrueColorImage, 32) => TextureFormat.BGRA32,
            _ => throw new Exception(),
        };

        int width = tga.Head.Width;
        int height = tga.Head.Height;
        var pixels = tga.Pixels;
        var level = new TextureLevel(width, height, pixels);
        if (tga.Head.ImageDescriptor.HasFlag(TgaFile.ImageDescriptor.ScreenOrigin))
        {
            level.FlipY();
        }
        var levels = new[] { level };

        return new Texture("", format, levels);
    }
}
