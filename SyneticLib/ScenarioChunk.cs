using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SyneticLib;

public class ScenarioChunk : Ressource
{
    public MeshSectionPtr Terrain;

    public int VertexIdxOffset;

    public Vector3 Center;
    public float Radius;

    public ScenarioChunk(ScenarioVariant parent, string path) : base(parent, path)
    {
    }

    protected override void OnLoad()
    {
        throw new NotImplementedException();
    }

    protected override void OnSave()
    {
        throw new NotImplementedException();
    }

    protected override void OnSeek()
    {
        throw new NotImplementedException();
    }
}
