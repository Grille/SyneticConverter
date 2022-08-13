using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Graphics;

namespace SyneticLib;
public class Texture : Ressource
{
    public byte[] PixelData;
    public int Id;

    public readonly TextureBuffer GLBuffer;

    public Texture(Ressource parent, string path) : base(parent, PointerType.File)
    {
        GLBuffer = new TextureBuffer(this);
        SourcePath = path;
    }

    protected override void OnLoad()
    {

    }

    protected override void OnSave()
    {

    }

    public void ExportFile(string path)
    {

    }

    protected override void OnSeek()
    {
        //throw new NotImplementedException();
    }
}
