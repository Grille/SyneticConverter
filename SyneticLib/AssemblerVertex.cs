using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public struct AssemblerVertex
{
    public readonly Vertex Vertex;

    public readonly int Hash;

    public AssemblerVertex(Vertex vertex)
    {
        Vertex = vertex;
        Hash = vertex.GetHashCode();
    }

    public override int GetHashCode()
    {
        return Hash;
    }
}
