using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;

namespace SyneticLib.IO;
public class TexturePtxSerializer : FileSerializer<PtxFile, Texture>
{
    protected override Texture OnDeserialize(PtxFile ptx)
    {
        var format = (ptx.Head.Compression, ptx.Head.BitPerPixel) switch
        {
            (0, 24) => TextureFormat.RGB24,
            (0, 32) => TextureFormat.RGBA32,
            (1, 24) => TextureFormat.RGB24Dxt1,
            (1, 32) => TextureFormat.RGBA32Dxt5,
            _ => throw new Exception(),
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
        var format = texture.Format switch
        {
            TextureFormat.RGB24 => (0, 24),
            TextureFormat.RGBA32 => (0, 32),
            TextureFormat.RGB24Dxt1 => (1, 24),
            TextureFormat.RGBA32Dxt5 => (1, 32),
            _ => throw new NotImplementedException(),
        };

        ptx.Head.Compression = (byte)format.Item1;
        ptx.Head.BitPerPixel = (byte)format.Item2;
        ptx.Head.Width = texture.Width;
        ptx.Head.Height = texture.Height;
        ptx.Head.MipMapLevels = (byte)texture.Levels.Length;
        ptx.Levels = new PtxFile.Level[ptx.Head.MipMapLevels];
        for (int i = 0; i < ptx.Levels.Length; i++)
        {
            ptx.Levels[i] = new PtxFile.Level(texture.Levels[i].Data);
        }
    }
}
