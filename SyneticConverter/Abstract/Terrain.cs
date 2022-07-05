using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticConverter;

public class Terrain
{
    public Mesh Mesh;

    public Chunk[,] Chunks;

    public Terrain(MaterialList materials)
    {
        Mesh = new Mesh(materials);
    }

    public void Chunkify()
    {
        var list = new List<Chunk>();

        //return list;
    }
}
