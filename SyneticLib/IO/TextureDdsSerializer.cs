using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.Files.Extra;

using static SyneticLib.Files.Extra.DdsFile;
using static SyneticLib.IO.Serializers;

namespace SyneticLib.IO;
public class TextureDdsSerializer : FileSerializer<DdsFile, Texture>
{
    protected override Texture OnDeserialize(DdsFile dds)
    {
        ref var head = ref dds.Head;

        var ddsLevels = dds.Levels;
        var levels = new TextureLevel[ddsLevels.Length];

        for (int i = 0; i < levels.Length; i++)
        {
            var src = ddsLevels[i];
            var level = new TextureLevel(src.Width, src.Height, src.Data, false);
            levels[i] = level;
        }

        var format = GetFormat(ref head);
        return new Texture(format, levels);
    }

    TextureFormat GetFormat(ref MHeader head)
    {
        ref var pFormat = ref head.PixelFormat;
        if (pFormat.FourCC == 0)
        {
            bool blueFirst = pFormat.BBitMask == 255;
            int size = (int)pFormat.Size;
            return (blueFirst, size) switch
            {
                (true, 24) => TextureFormat.Bgr24,
                (true, 32) => TextureFormat.Bgra32,
                (false, 24) => TextureFormat.Rgb24,
                (false, 32) => TextureFormat.Rgba32,
                (_, 8) => TextureFormat.R8,
                _ => throw new NotSupportedException(),
            };
        }
        else if (pFormat.FourCC == MFourCharCode.DXT1)
        {
            return TextureFormat.Rgb24Dxt1;
        }
        else if (pFormat.FourCC == MFourCharCode.DXT5)
        {
            return TextureFormat.Rgba32Dxt5;
        }
        else
        {
            throw new NotSupportedException(pFormat.FourCC.ToString());
        }
    }

    protected override void OnSerialize(DdsFile dds, Texture texture)
    {
        ref var head = ref dds.Head;

        dds.Magic = MagicValue;

        head.Size = 124;
        head.Caps1 = MHeaderCaps1.Complex | MHeaderCaps1.Texture | MHeaderCaps1.Mipmap;
        head.Flags = MHeaderFlags.Width | MHeaderFlags.Height | MHeaderFlags.PixelFormat | MHeaderFlags.MipmapCount | MHeaderFlags.LinearSize;

        SetFormat(ref head, texture.Format);

        head.Width = texture.Width;
        head.Height = texture.Height;
        head.PitchOrLinearSize = texture.Width * texture.Height * (int)(head.PixelFormat.RGBBitCount / 8);
        head.MipMapCount = texture.Levels.Length;


        var levels = new Level[texture.Levels.Length];
        for (int i = 0; i < texture.Levels.Length; i++)
        {
            var src = texture.Levels[i];
            levels[i] = new Level(src.Data, src.Width, src.Height);
        }
        dds.Levels = levels;
    }

    void SetFormat(ref MHeader head, TextureFormat format)
    {
        ref var pFormat = ref head.PixelFormat;
        switch (format)
        {
            case TextureFormat.Rgb24Dxt1:
            {
                pFormat.Flags = MPixelFormatFlags.FourCC;
                pFormat.FourCC = MFourCharCode.DXT1;
                break;
            }
            case TextureFormat.Rgba32Dxt5:
            {
                pFormat.Flags = MPixelFormatFlags.FourCC;
                pFormat.FourCC = MFourCharCode.DXT5;
                break;
            }
            default:
            {
                int size = format.BitSize();
                pFormat.Size = (uint)size;
                pFormat.RGBBitCount = (uint)size;
                pFormat.Flags = MPixelFormatFlags.RGB | MPixelFormatFlags.AlphaPixels;

                if (format == TextureFormat.Bgr24 || format == TextureFormat.Bgra32)
                {
                    pFormat.BBitMask = 255u << 00;
                    pFormat.GBitMask = 255u << 08;
                    pFormat.RBitMask = 255u << 16;
                }
                else
                {
                    pFormat.RBitMask = 255u << 00;
                    pFormat.GBitMask = 255u << 08;
                    pFormat.BBitMask = 255u << 16;
                }
                pFormat.ABitMask = 255u << 24;
                break;
            }
        }
    }
}
