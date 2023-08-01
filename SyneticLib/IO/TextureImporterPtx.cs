using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.LowLevel.Files;

namespace SyneticLib.IO;
public class TextureImporterPtx : TextureImporter
{
    private PtxFile ptx;
    public TextureImporterPtx(Texture target) : base(target)
    {
        ptx = new();
        ptx.Path = target.SourcePath;
    }

    protected override void OnLoad()
    {
        ptx.Load();

        Target.Format = (ptx.Head.Compression, ptx.Head.BitPerPixel) switch
        {
            (0, 32) => TextureFormat.Rgba32,
            (1, 32) => TextureFormat.Rgba32Dxt5,
            _ => throw new Exception(),
        };

        Target.Width = ptx.Head.Width;
        Target.Height = ptx.Head.Height;

        Target.Levels = new TextureLevel[ptx.Head.MipMapLevels];
        for (var i = 0; i < Target.Levels.Length; i++)
        {
            var level = Target.Levels[i] = new();
            level.PixelData = ptx.Levels[i].Decoded;
        }
    }
}
