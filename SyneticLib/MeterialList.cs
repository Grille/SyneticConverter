using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class MaterialList : RessourceList<Material>
{
    public TextureDirectory Textures { get; }

    public MaterialList(Ressource parent, TextureDirectory textures) : base(parent, parent.ChildPath("Materials"))
    {
        Textures = textures;
    }

    public void DisposeAll()
    {
        foreach (var material in this)
        {

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

