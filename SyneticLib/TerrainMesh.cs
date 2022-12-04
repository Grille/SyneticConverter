using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.Graphics;

namespace SyneticLib;
public class TerrainMesh : Mesh
{
    public TerrainMesh(Terrain parrent) : base(parrent, parrent.ChildPath("Terrain"), PointerType.Virtual)
    {
        GLBuffer = new TerrainBuffer(this);
    }
}
