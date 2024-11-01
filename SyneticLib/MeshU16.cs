using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;

internal class MeshU16
{
    public int[] Offsets { get; }
    public IndexedMesh Mesh { get; }

    public MeshU16(IndexedMesh mesh, int[] offsets)
    {
        Offsets = offsets;
        Mesh = mesh;
    }



}
