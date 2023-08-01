using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using static System.IO.Path;

using SyneticLib.IO;
namespace SyneticLib.Locations;

public class TextureDirectory : RessourceDirectory<Texture>
{
    static bool filter(string path) => File.Exists(path) && GetExtension(path).ToLower() == ".ptx";

    static Texture constructor(string path) => TextureImporterPtx.LoadPtxTexture(path);

    public TextureDirectory(string path) :
        base(path, filter, constructor)
    { }
}

