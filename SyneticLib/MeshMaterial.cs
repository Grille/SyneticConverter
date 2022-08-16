using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.Graphics;

namespace SyneticLib;
public class MeshMaterial : Ressource
{
    public MeshMaterialProgram GLProgram;
    public MeshMaterial(Ressource parent) : base(parent)
    {
        GLProgram = new MeshMaterialProgram(this);
    }


    public static readonly MeshMaterial Default = new MeshMaterial(null);

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
