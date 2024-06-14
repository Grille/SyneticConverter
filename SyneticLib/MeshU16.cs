using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;

internal class MeshU16
{
    public int[] Offsets { get; }
    public Mesh Mesh { get; }

    public MeshU16(Mesh mesh, int[] offsets)
    {
        Offsets = offsets;
        Mesh = mesh;
    }



}
