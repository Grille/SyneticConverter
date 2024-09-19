using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public struct AssemblerTriangle
{
    public readonly AssemblerVertex Vertex0;
    public readonly AssemblerVertex Vertex1;
    public readonly AssemblerVertex Vertex2;

    public readonly int MaterialID0;

    public readonly int Hash;

    public AssemblerTriangle(Span<Vertex> vertices, IdxTriangleInt32 triangle, int materialID)
    {
        Vertex0 = new AssemblerVertex( vertices[triangle.X]);
        Vertex1 = new AssemblerVertex(vertices[triangle.Y]);
        Vertex2 = new AssemblerVertex(vertices[triangle.Z]);

        MaterialID0 = materialID;

        Hash = 0;
    }




    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}


