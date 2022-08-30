using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib;
public class ModelDirectory : RessourceDirectory<Model>
{
    static bool filter(string path) => File.Exists(path) && Path.GetExtension(path).ToLower() == ".mox";

    public ModelDirectory(Ressource parent, TextureDirectory textures, string path) : base(parent, path, filter, (a) => new Model(parent, a))
    {
        //TextureFolder = new();
    }
}
