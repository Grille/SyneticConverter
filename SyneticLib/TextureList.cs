using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib;
public class TextureDirectory : RessourceDirectory<Texture>
{
    public TextureDirectory(Ressource parent, string path) : base(parent, path)
    {
        Filter = (str) => File.Exists(str);

        Constructor = (path) =>
        {
            var texture = new Texture();
            return texture;
        };
    }
}

