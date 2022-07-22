using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class Texture : RessourceFile
{
    public byte[] PixelData;
    public int Id;

    public Texture()
    {

    }

    public static Texture FromFile(string path)
    {
        var texture = new Texture();
        texture.Load(path);
        return texture;
    }

    protected override void OnLoad(string path)
    {
        SrcPath = path;

        if (PointerState == PointerState.Exists)
        {
            /* do things */

            //DataState = DataState.Loaded;
        }
    }

    protected override void OnSave(string path)
    {

    }

    public void ExportFile(string path)
    {

    }

}
