using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.Graphics;

namespace SyneticLib;

public class Terrain : Ressource
{
    public string Name;
    public int[] Indecies;
    public MeshVertex[] Vertices;
    //public Vector3Int[] Poligons;
    public MaterialRegion[] MaterialRegion;
    public RessourceList<TerrainMaterial> Materials;

    public TerrainChunk[,] Chunks;

    public TerrainBuffer GLBuffer;

    public Terrain(ScenarioVariant parrent) : base(parrent)
    {
        GLBuffer = new TerrainBuffer(this);
    }

    public void CalculateChunks()
    {
        var list = new List<TerrainChunk>();

        //return list;
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
        //throw new NotImplementedException();
    }
}
