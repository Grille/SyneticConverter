using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SyneticLib;

public class ScenarioChunk : Ressource
{
    public TerrainMesh Terrain;

    public int VertexIdxOffset;

    public Vector3 Center;
    public float Radius;

    public ScenarioChunk(Scenario parent, string path) : base(parent, path)
    {
    }
}
