using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SyneticLib;

public class TerrainChunkInfo
{
    public int ElementOffset;
    public int ElementCount;
    public int MaterialOffset;
    public int MaterialCount;

    public int VertexIdxOffset;

    public Vector3 Position;
    public float Size;
}
