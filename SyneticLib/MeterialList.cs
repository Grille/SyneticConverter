using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class MaterialList : RessourceList<Material>
{
    public MaterialList(Ressource parent) : base(parent, parent.ChildPath("Materials"))
    {

    }

    public void DisposeAll()
    {
        foreach (var material in this)
        {
            material.GLProgram.Dispose();
        }
    }

    public Material GetByID(int id)
    {
        foreach (var material in this)
        {
            if (material.ID == id)
                return material;
        }
        throw new KeyNotFoundException();
    }


}

