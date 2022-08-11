using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class Texture : Ressource
{
    public byte[] PixelData;
    public int Id;

    public Texture() : base(null, PointerType.File)
    {

    }

    public static Texture FromFile(string path)
    {
        var texture = new Texture();
        texture.SourcePath = path;
        texture.Load();
        return texture;
    }

    protected override void OnLoad()
    {
        if (PointerState == PointerState.Exists)
        {
            /* do things */

            //DataState = DataState.Loaded;
        }
    }

    protected override void OnSave()
    {

    }

    public void ExportFile(string path)
    {

    }

    protected override void OnSeek()
    {
        throw new NotImplementedException();
    }
}
