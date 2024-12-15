using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;
using SyneticLib.Utils;

using static SyneticLib.Files.TruevisionTgaFile;

namespace SyneticLib.IO;
public class TextureTgaSerializer : FileSerializer<TruevisionTgaFile, Texture>
{
    protected override Texture OnDeserialize(TruevisionTgaFile tga)
    {
        var format = (tga.Head.ImageType, tga.Head.BitsPerPixel) switch
        {
            (ImageType.GrayScale, 8) => TextureFormat.R8,
            (ImageType.TrueColorImage, 8) => TextureFormat.R8,
            (ImageType.TrueColorImage, 24) => TextureFormat.Bgr24,
            (ImageType.TrueColorImage, 32) => TextureFormat.Bgra32,
            _ => throw new Exception(),
        };

        int width = tga.Head.Width;
        int height = tga.Head.Height;
        var pixels = FlipY(tga.Pixels, width, height);
        var level = new TextureLevel(width, height, pixels);
        if (tga.Head.ImageDescriptor.HasFlag(ImageDescriptor.ScreenOrigin))
        {
            level.FlipY();
        }
        var levels = new[] { level };

        return new Texture(format, levels);
    }

    protected override void OnSerialize(TruevisionTgaFile tga, Texture texture)
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
            case TextureFormat.Bgr24:
            {
                type = ImageType.TrueColorImage;
                bits = 24;
                data = texture.MainSurfaceData;
                break;
            }
            case TextureFormat.Bgra32:
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
        tga.Head.OriginX = 0;
        tga.Head.OriginY = 0;
        tga.Head.Width = (ushort)texture.Width;
        tga.Head.Height = (ushort)texture.Height;
        tga.Pixels = FlipY(data, texture.Width, texture.Height);
    }

    static byte[] FlipY(byte[] data, int width, int height)
    {
        int stride = data.Length / (width * height);

        byte[] reversed = new byte[data.Length];

        var lines = data.Length / stride;

        for (var line = 0; line < lines; line++)
        {
            Array.Copy(data, data.Length - ((line + 1) * stride), reversed, line * stride, stride);
        }

        return reversed;
    }
}
