using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class MaterialList : List<Material>
{
    TextureFolder textures;
    public MaterialList(TextureFolder textures)
    {
        this.textures = textures;
    }

    public void InitAllGlMaterials()
    {
        foreach (var material in this)
        {
            if (!material.GLShader.IsInitialized)
                material.GLShader.Initialize();
        }
    }

    public void DisposeAll()
    {
        foreach (var material in this)
        {
            material.GLShader.Dispose();
        }
    }


}

