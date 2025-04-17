using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;
using SyneticLib.IO.Generic;
using SyneticLib.Utils;

using static SyneticLib.IO.Serializers;

namespace SyneticLib.IO;
public class TexturePtxSerializer : FileSerializer<PtxFile, Texture>
{
    protected override Texture OnDeserialize(PtxFile ptx)
    {
        var format = (ptx.Head.Compression, ptx.Head.BitPerPixel) switch
        {
            (0, 24) => TextureFormat.Bgr24,
            (0, 32) => TextureFormat.Bgra32,
            (1, 24) => TextureFormat.Rgb24Dxt1,
            (1, 32) => TextureFormat.Rgba32Dxt5,
            _ => throw new InvalidDataException(),
        };

        int width = ptx.Head.Width;
        int height = ptx.Head.Height;

        var levels = new TextureLevel[ptx.Head.MipMapLevels];
        for (var i = 0; i < levels.Length; i++)
        {
            var pixels = ptx.Levels[i].Pixels;

            levels[i] = new TextureLevel(width, height, pixels);

            width /= 2;
            height /= 2;
        }

        return new Texture(format, levels);
    }

    protected override void OnSerialize(PtxFile ptx, Texture texture)
    {
        ref var head = ref ptx.Head;

        bool toBgra = false;

        (int dxtCompression, int bits) = texture.Format switch
        {
            TextureFormat.Rgb24Dxt1 => (1, 24),
            TextureFormat.Rgba32Dxt5 => (1, 32),
            TextureFormat.Bgr24 => (0, 32),
            TextureFormat.Bgra32 => (0, 32),
            _ => (0, 0),
        };

        if (bits == 0)
        {
            toBgra = true;
            bits = 32;
        }

        ptx.Head.Compression = (byte)dxtCompression;
        ptx.Head.BitPerPixel = (byte)bits;
        ptx.Head.Width = texture.Width;
        ptx.Head.Height = texture.Height;
        ptx.Head.MipMapLevels = (byte)texture.Levels.Length;
        ptx.Levels = new PtxFile.Level[ptx.Head.MipMapLevels];

        for (int i = 0; i < ptx.Levels.Length; i++)
        {
            var data = toBgra == true ? texture.DataToBgra32(i) : texture.Levels[i].Data;
            ptx.Levels[i] = new PtxFile.Level(data);
        }
    }
}
