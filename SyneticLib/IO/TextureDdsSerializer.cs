using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.Files.Extra;

using static SyneticLib.Files.Extra.DdsFile;

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

        TextureFormat format;

        if (head.PixelFormat.FourCC == 0)
        {
            throw new NotSupportedException(head.PixelFormat.Size.ToString());
        }
        else if (head.PixelFormat.FourCC == MFourCharCode.DXT1)
        {
            format = TextureFormat.RGB24Dxt1;
        }
        else if (head.PixelFormat.FourCC == MFourCharCode.DXT5)
        {
            format = TextureFormat.RGBA32Dxt5;
        }
        else
        {
            throw new NotSupportedException(head.PixelFormat.FourCC.ToString());
        }

        return new Texture(format, levels);
    }

    protected override void OnSerialize(DdsFile dds, Texture texture)
    {
        ref var head = ref dds.Head;

        dds.Magic = MagicValue;

        head.Size = 124;
        head.Caps1 = MHeaderCaps1.Complex | MHeaderCaps1.Texture | MHeaderCaps1.Mipmap;
        head.Flags = MHeaderFlags.Width | MHeaderFlags.Height | MHeaderFlags.PixelFormat | MHeaderFlags.MipmapCount | MHeaderFlags.LinearSize;
        head.Width = texture.Width;
        head.Height = texture.Height;
        head.PitchOrLinearSize = texture.Width * texture.Height;
        head.MipMapCount = texture.Levels.Length;

        switch (texture.Format)
        {
            case TextureFormat.RGB24Dxt1:
            {
                head.PixelFormat.FourCC = MFourCharCode.DXT1;
                break;
            }
            case TextureFormat.RGBA32Dxt5:
            {
                head.PixelFormat.FourCC = MFourCharCode.DXT5;
                break;
            }
            default:
            {
                goto case TextureFormat.RGBA32;
            }
            case TextureFormat.RGBA32:
            {
                throw new InvalidOperationException();
            }
        }

        var levels = new Level[texture.Levels.Length];
        for (int i = 0; i < texture.Levels.Length; i++)
        {
            var src = texture.Levels[i];
            levels[i] = new Level(src.Data, src.Width, src.Height);
        }
        dds.Levels = levels;
    }
}
