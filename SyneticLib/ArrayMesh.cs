using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

namespace SyneticLib;

public class ArrayMesh : SyneticObject
{
    readonly Vertex[] _vertices;

    public Span<Vertex> Vertices => _vertices.AsSpan();

    public Span<VertexTriangle> Triangles => MemoryMarshal.Cast<Vertex, VertexTriangle>(Vertices);

    public ArrayMesh(Vertex[] vertices)
    {
        _vertices = vertices;
    }

    public ArrayMesh(ReadOnlySpan<Vertex> vertices, ReadOnlySpan<int> indices)
    {
        _vertices = new Vertex[indices.Length];

        for (int i = 0; i < indices.Length; i++)
        {
            _vertices[i] = vertices[indices[i]];
        }
    }

    public struct VertexTriangle
    {
        public Vertex X, Y, Z;

        public Vector3 Center => (X.Position + Y.Position + Z.Position) / 3f;
    }
}
