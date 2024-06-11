using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SyneticLib;

public class ScenarioChunk : SyneticObject
{
    public Model Terrain;

    public Vector3 Center;
    public float Radius;

    public ScenarioChunk(int x, int y) : base($"x{x},y{y}")
    {

    }
}
