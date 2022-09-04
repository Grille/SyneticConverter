using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib;
public class TextureDirectory : RessourceDirectory<Texture>
{
    static bool filter(string path) => File.Exists(path) && Path.GetExtension(path).ToLower() == ".ptx";

    public TextureDirectory(Ressource parent, string path) : base(parent, path, filter, (a) => new Texture(parent, a))
    {

    }
}

