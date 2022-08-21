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
    public TerrainMaterial Material;

    public MaterialRegion()
    {

    }

    public MaterialRegion(int offset, int count)
    {
        Offset = offset;
        Count = count;
        Material = TerrainMaterial.Default;
    }   
}
