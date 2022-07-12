using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class PropClass
{
    public string Name;

    public int AnimationMode;
    public int ColliShape;

    public Mesh Mesh;
    public MaterialList Materials;

    public PropClass(string name, TextureFolder textures)
    {
        Name = name;
        Materials = new MaterialList(textures);
        Mesh = new(Materials);
    }

}
