using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SyneticLib;

public class ScenarioChunk : Ressource
{
    public int VertexIdxOffset;

    public Vector3 Center;
    public float Radius;

    public int TerrainMaterialRegionOffset;
    public int TerrainMaterialRegionCount;

    public ScenarioChunk(Scenario parent, int x, int y) : base(parent, $"x{x},y{y}")
    {

    }
}
