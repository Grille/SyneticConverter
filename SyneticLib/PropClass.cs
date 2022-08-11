using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class PropClass : Ressource
{
    public string Name;

    public int AnimationMode;
    public int ColliShape;

    public Mesh Mesh;
    public MaterialList Materials;

    public PropClass(Ressource parent, TextureDirectory textures, string name) : base(parent, PointerType.Virtual)
    {
        Name = name;
        Materials = new MaterialList(textures);
        Mesh = new(Materials);
    }

    protected override void OnLoad()
    {
        throw new NotImplementedException();
    }

    protected override void OnSave()
    {
        throw new NotImplementedException();
    }

    protected override void OnSeek()
    {
        throw new NotImplementedException();
    }
}
