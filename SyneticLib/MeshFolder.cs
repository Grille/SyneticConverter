using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib;
public class MeshDirectory : RessourceDirectory<Mesh>
{
    static bool filter(string path) => File.Exists(path) && Path.GetExtension(path).ToLower() == ".mox";

    public MeshDirectory(Ressource parent, TextureDirectory textures, string path) : base(parent, path, filter, (a) => new Mesh(parent, a))
    {
        //TextureFolder = new();
    }
}
