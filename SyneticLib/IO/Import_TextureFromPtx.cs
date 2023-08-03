using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.LowLevel.Files;

namespace SyneticLib.IO;
public static partial class Imports
{

    public static Texture LoadPtxTexture(string path)
    {
        var ptx = new PtxFile();
        ptx.Load(path);

        var name = Path.GetFileNameWithoutExtension(path);

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
            var pixels = ptx.Levels[i].Decoded;

            int lwidth = width / (i + 1);
            int lheight = height / (i + 1);

            levels[i] = new TextureLevel(lwidth, lheight, pixels);
        }

        return new Texture(name, format, levels);
    }
}
