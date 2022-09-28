using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.IO.Synetic.Files;

namespace SyneticLib.IO.Synetic;
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

        Target.Compressed = ptx.Head.Compression == 1;
        Target.Bits = ptx.Head.BitPerPixel;

        Target.Width = ptx.Head.Width;
        Target.Height = ptx.Head.Height;

        Target.Levels = new TextureLevel[ptx.Head.MipMapLevels];
        for (int i = 0; i < Target.Levels.Length; i++)
        {
            var level = Target.Levels[i] = new();
            level.PixelData = ptx.Levels[i].Decoded;
        }
    }
}
