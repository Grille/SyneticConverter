using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using SyneticLib.Graphics;

namespace SyneticLib;
public class ModelMaterial : Material {

    public Color Diffuse;

    public ModelMaterial() : base(null, "MeshMaterial")
    {
        GLProgram = new MeshMaterialProgram(this);
    }

    public ModelMaterial(Ressource parent) : base(parent, parent.ChildPath("MeshMaterial"))
    {
        GLProgram = new MeshMaterialProgram(this);
    }


    public static readonly ModelMaterial Default = new ModelMaterial(GameDirectory.Global);

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
