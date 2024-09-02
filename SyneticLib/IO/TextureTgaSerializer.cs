using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;
using SyneticLib.Utils;

using static SyneticLib.Files.TgaFile;

namespace SyneticLib.IO;
public class TextureTgaSerializer : FileSerializer<TgaFile, Texture>
{
    protected override Texture OnDeserialize(TgaFile tga)
    {
        var format = (tga.Head.ImageType, tga.Head.BitsPerPixel) switch
        {
            (ImageType.GrayScale, 8) => TextureFormat.R8,
            (ImageType.TrueColorImage, 8) => TextureFormat.R8,
            (ImageType.TrueColorImage, 24) => TextureFormat.RGB24,
            (ImageType.TrueColorImage, 32) => TextureFormat.BGRA32,
            _ => throw new Exception(),
        };

        int width = tga.Head.Width;
        int height = tga.Head.Height;
        var pixels = tga.Pixels;
        var level = new TextureLevel(width, height, pixels);
        if (tga.Head.ImageDescriptor.HasFlag(ImageDescriptor.ScreenOrigin))
        {
            level.FlipY();
        }
        var levels = new[] { level };

        return new Texture(format, levels);
    }

    protected override void OnSerialize(TgaFile tga, Texture texture)
    {
        ImageType type;
        byte bits;
        byte[] data;

        switch (texture.Format)
        {
            case TextureFormat.R8:
            {
                type = ImageType.GrayScale;
                bits = 8;
                data = texture.MainSurfaceData;
                break;
            }
            case TextureFormat.BGRA32:
            {
                type = ImageType.TrueColorImage;
                bits = 32;
                data = texture.MainSurfaceData;
                break;
            }
            default:
            {
                type = ImageType.TrueColorImage;
                bits = 32;
                data = texture.DataToBgra32(0);
                break;
            }
        }
        tga.Head.ImageType = type;
        tga.Head.BitsPerPixel = bits;
        tga.Head.Width = (ushort)texture.Width;
        tga.Head.Height = (ushort)texture.Height;
        tga.Pixels = texture.Levels[0].Data;
    }
}
