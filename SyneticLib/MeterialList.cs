using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.Locations;

namespace SyneticLib;
public class MaterialList : RessourceList<Material>
{
    public TextureDirectory Textures { get; }

    public MaterialList(TextureDirectory textures) 
    {
        Textures = textures;
    }
}

