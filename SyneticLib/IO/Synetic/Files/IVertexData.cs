﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.IO.Synetic.Files;
public interface IVertexData
{
    public int[] IndicesOffset { get; set; }
    public Vertex[] Vertecis { get; set; }
}
