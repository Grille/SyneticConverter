using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.IO.Synetic;

namespace SyneticLib.IO;
public abstract class TextureImporter
{
    protected Texture Target;

    public TextureImporter(Texture target)
    {
        Target = target;
    }

    protected abstract void OnLoad();

    public void Load()
    {
        //try
        //{
            OnLoad();
            Target.DataState = DataState.Loaded;
        //}
        //catch
        //{
        //    Target.DataState = DataState.Error;
        //}
    }
}
