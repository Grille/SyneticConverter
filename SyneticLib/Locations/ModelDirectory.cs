using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SyneticLib.Locations;

using static System.IO.Path;
using SyneticLib.IO;

namespace SyneticLib;
public class ModelDirectory : LazyRessourceDirectory<Model>
{
    static bool filter(string path) => File.Exists(path) && GetExtension(path).ToLower() == ".mox";

    public ModelDirectory(TextureDirectory textures, string path) : base(path, filter, (a) => Imports.LoadModelFromMox(path, textures))
    {
        //TextureFolder = new();
    }
}
