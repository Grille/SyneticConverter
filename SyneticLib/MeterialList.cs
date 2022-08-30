using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class TerrainMaterialList : RessourceList<TerrainMaterial>
{
    TextureDirectory textures;
    public TerrainMaterialList(ScenarioVariant parent, TextureDirectory textures) : base(parent, parent.ChildPath("TerrainMaterialList"))
    {
        this.textures = textures;
    }

    public void DisposeAll()
    {
        foreach (var material in this)
        {
            material.GLProgram.Dispose();
        }
    }


}

