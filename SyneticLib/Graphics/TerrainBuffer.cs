using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Graphics;
public class TerrainBuffer : GLStateObject
{
    Terrain owner;
    public TerrainBuffer(Terrain terrain)
    {
        owner = terrain;
    }

    protected override void OnBind()
    {
        throw new NotImplementedException();
    }

    protected override void OnCreate()
    {
        throw new NotImplementedException();
    }

    protected override void OnDestroy()
    {
        throw new NotImplementedException();
    }
}
