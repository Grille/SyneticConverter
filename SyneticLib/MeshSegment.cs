using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

    public Span<IdxTriangleInt32> Triangles => Mesh.Triangles.AsSpan(Start, Length);

    public Span<int> Indices => Mesh.Indices.Slice(Start * 3, Length * 3);

    public MeshSegment(IndexedMesh mesh)
    {
        Mesh = mesh;
        Offset = 0;
        Start = 0;
        Length = mesh.Triangles.Length;

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

        if (End > mesh.Triangles.Length)
        {
            throw new ArgumentOutOfRangeException("End must be <= Mesh.Indices.Length", nameof(length));
        }

        UpdateBoundingBox();
    }

    public void ApplyOffset()
    {

    }

    public void UpdateBoundingBox()
    {
        BoundingBox = new BoundingBox(Vertices, Triangles, Offset);
    }

    public IndexedMesh ToIndexedMesh()
    {
        var range = Mesh.GetIndicesRange(Start, Length, Offset);

        int start = range.Start.Value;
        int length = range.End.Value - start;

        var vertecies = Vertices.Slice(start, length).ToArray();
        var indices = Indices.ToArray();

        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] -= Offset;
        }

        var triangles = MemoryMarshal.Cast<int, IdxTriangleInt32>(indices.AsSpan()).ToArray();

        return new IndexedMesh(vertecies, triangles);
    }

    public ArrayMesh ToArrayMesh()
    {
        return new ArrayMesh(Vertices, Indices);
    }
}
