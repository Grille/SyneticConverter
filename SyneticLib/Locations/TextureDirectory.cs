using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


using SyneticLib.IO;
namespace SyneticLib.Locations;

public class TextureDirectory : LazyRessourceDirectory<Texture>
{
    static bool filter(string path) => File.Exists(path) && Path.GetExtension(path).ToLower() == ".ptx";

    static Texture constructor(string path)
    {
        var texture = Serializers.Texture.Ptx.Load(path);
        texture.Name = Path.GetFileNameWithoutExtension(path);
        return texture;
    }

    public TextureDirectory(string path) :
        base(path, filter, constructor)
    { }
}

