using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;

public class MaterialRegion
{
    public int Offset;
    public int Count;
    public Material Material;


    public MaterialRegion(int offset, int count, Material material)
    {
        Offset = offset;
        Count = count;
        Material = material;
    }   
}
