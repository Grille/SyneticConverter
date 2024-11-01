using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class MeshSegment
{
    public IndexedMesh Mesh { get; }

    public int Offset { get; }
    public int Start { get; }
    public int Length { get; }

    public int End => Start + Length;

    public BoundingBox BoundingBox;

    public Span<Vertex> Vertices => new Span<Vertex>(Mesh.Vertices);

    public Span<IdxTriangleInt32> Indices => new Span<IdxTriangleInt32>(Mesh.Indices, Start, Length);

    public MeshSegment(IndexedMesh mesh)
    {
        Mesh = mesh;
        Offset = 0;
        Start = 0;
        Length = mesh.Indices.Length;

        UpdateBoundingBox();
    }

    public MeshSegment(IndexedMesh mesh, int start, int length, int offset = 0)
    {
        Mesh = mesh;
        Offset = offset;
        Start = start;
        Length = length;

        if (Start < 0)
        {
            throw new ArgumentOutOfRangeException("Start must be > 0", nameof(start));
        }

        if (End > mesh.Indices.Length)
        {
            throw new ArgumentOutOfRangeException("End must be <= Mesh.Indices.Length", nameof(length));
        }

        UpdateBoundingBox();
    }

    public void UpdateBoundingBox()
    {
        BoundingBox = new BoundingBox(Vertices, Indices, Offset);
    }

    public IndexedMesh ToMesh()
    {
        var vertecies = Vertices.ToArray();
        var indices = Indices.ToArray();

        var mesh = new IndexedMesh(vertecies, indices);
        mesh.ApplyOffset(Offset);

        return mesh;
    }
}
