using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using SyneticLib.Graphics;

namespace SyneticLib;

public class Terrain : Ressource
{
    public float GridSize = 0;

    public RessourceList<TerrainMesh> Chunks;
    public Terrain(Scenario parrent) : base(parrent, parrent.ChildPath("Terrain"), PointerType.Virtual)
    {
        Chunks = new(this, "Chunks");
    }

    public TerrainMesh this[int x, int y]
    {
        get => Chunks[CalcIdx(x, y)];
    }

    public int CalcIdx(int x, int y)
    {
        return 0;
    }

    public void ChunkifyMesh(TerrainMesh mesh)
    {
        var list = new List<ScenarioChunk>();

        //return list;
    }

    public TerrainMesh JoinChunks()
    {
        throw new NotImplementedException();
    }
}
