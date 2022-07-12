using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class MeshFolder : List<Mesh>
{
    public TextureFolder TextureFolder;

    public MeshFolder()
    {
        TextureFolder = new();
    }

    public void InitByPath(string path)
    {

    }

    public void LoadAllTextures()
    {

    }
}
