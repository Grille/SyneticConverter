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
    public static void TextureAsPtx(this Texture texture, string filePath)
    {
        var ptx = new PtxFile();
        TextureAsPtx(texture, ptx);
        ptx.Save(filePath);
    }

    public static void TextureAsPtx(this Texture texture, PtxFile ptx)
    {
        var format = texture.Format switch
        {
            TextureFormat.RGB24 => (0, 24),
            TextureFormat.RGBA32 => (0, 32),
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
            ptx.Levels[i] = new PtxFile.Level( texture.Levels[i].Data);
        }
    }
}
