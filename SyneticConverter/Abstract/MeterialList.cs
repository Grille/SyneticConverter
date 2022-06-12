using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticConverter;
public class MaterialList : List<Material>
{
    TextureList textures;
    public MaterialList(TextureList textures)
    {
        this.textures = textures;
    }
}

