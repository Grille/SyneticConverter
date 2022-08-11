using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class MeshDirectory : RessourceDirectory<Mesh>
{
    public TextureDirectory TextureFolder;

    public MeshDirectory(Ressource parent, TextureDirectory textures, string path): base(parent, path)
    {
        //TextureFolder = new();
    }
}
