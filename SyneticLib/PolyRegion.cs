﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;

public class MaterialRegion
{
    public int ElementOffset;
    public int ElementCount;
    public Material Material;


    public MaterialRegion(int offset, int count, Material material)
    {
        ElementOffset = offset;
        ElementCount = count;
        Material = material;
    }   
}
