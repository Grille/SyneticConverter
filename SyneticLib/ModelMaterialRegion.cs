using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;

public class ModelMaterialRegion
{
    public int ElementStart;
    public int ElementCount;
    public Material Material;


    public ModelMaterialRegion(int offset, int count, Material material)
    {
        ElementStart = offset;
        ElementCount = count;
        Material = material;
    }   
}
