using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;

namespace SyneticLib.IO;
public class TextureTgaSerializer : FileSerializer<TgaFile, Texture>
{
    public override Texture OnDeserialize(TgaFile tga)
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

    protected override void OnSerialize(TgaFile tga, Texture texture)
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
