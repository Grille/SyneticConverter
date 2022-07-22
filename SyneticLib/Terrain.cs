using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;

public class Terrain
{
    public string Name;
    public int[] Indecies;
    public MeshVertex[] Vertices;
    public Vector3Int[] Poligons;
    public MaterialRegion[] MaterialRegion;
    public MaterialList Materials;

    public TerrainChunk[,] Chunks;

    public Terrain(MaterialList materials)
    {
        Materials = materials;
    }

    public void CalculateChunks()
    {
        var list = new List<TerrainChunk>();

        //return list;
    }
}
