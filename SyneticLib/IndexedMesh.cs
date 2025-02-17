using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK.Mathematics;

namespace SyneticLib;

public class IndexedMesh : SyneticObject
{
    public Vertex[] Vertices { get; set; }
    public IdxTriangleInt32[] Triangles { get; set; }

    public Span<int> Indices => MemoryMarshal.Cast<IdxTriangleInt32, int>(Triangles.AsSpan());

    public MeshIndexType IndexType { get; private set; }

    public IndexedMesh(Vertex[] vertices, IdxTriangleInt32[] triangles, MeshIndexType type = MeshIndexType.UInt32)
    {
        Vertices = vertices;
        Triangles = triangles;

        if (type == MeshIndexType.Unknown)
        {
            UpdateIndicesType();
        }
        else
        {
            IndexType = type;
        }
    }

    public IndexedMesh(IndexedMesh mesh)
    {
        Vertices = mesh.Vertices.ToArray();
        Triangles = mesh.Triangles.ToArray();
        IndexType = mesh.IndexType;
    }

    public static IndexedMesh CreateBox()
    {
        // Define the vertices of the box (cube), assuming a unit cube centered at the origin
        var vertices = new Vertex[]
        {
            new Vector3(-0.5f, -0.5f,  0.5f), // Front-bottom-left
            new Vector3( 0.5f, -0.5f,  0.5f), // Front-bottom-right
            new Vector3( 0.5f,  0.5f,  0.5f), // Front-top-right
            new Vector3(-0.5f,  0.5f,  0.5f), // Front-top-left
            new Vector3(-0.5f, -0.5f, -0.5f), // Back-bottom-left
            new Vector3( 0.5f, -0.5f, -0.5f), // Back-bottom-right
            new Vector3( 0.5f,  0.5f, -0.5f), // Back-top-right
            new Vector3(-0.5f,  0.5f, -0.5f)  // Back-top-left
        };

        // Define the indices for the triangles (2 per face, 6 faces)
        var indices = new IdxTriangleInt32[]
        {
            // Front face (0, 1, 2), (2, 3, 0)
            new IdxTriangleInt32(0, 1, 2),
            new IdxTriangleInt32(2, 3, 0),

            // Back face (5, 4, 7), (7, 6, 5)
            new IdxTriangleInt32(5, 4, 7),
            new IdxTriangleInt32(7, 6, 5),

            // Left face (4, 0, 3), (3, 7, 4)
            new IdxTriangleInt32(4, 0, 3),
            new IdxTriangleInt32(3, 7, 4),

            // Right face (1, 5, 6), (6, 2, 1)
            new IdxTriangleInt32(1, 5, 6),
            new IdxTriangleInt32(6, 2, 1),

            // Top face (3, 2, 6), (6, 7, 3)
            new IdxTriangleInt32(3, 2, 6),
            new IdxTriangleInt32(6, 7, 3),

            // Bottom face (4, 5, 1), (1, 0, 4)
            new IdxTriangleInt32(4, 5, 1),
            new IdxTriangleInt32(1, 0, 4)
        };

        // Return a new Mesh object
        return new IndexedMesh(vertices, indices, MeshIndexType.UInt16);
    }

    public void UpdateIndicesType()
    {
        var range = GetIndicesRange(0, Indices.Length);
        IndexType = range.End.Value > ushort.MaxValue ? MeshIndexType.UInt32 : MeshIndexType.UInt16;
    }

    public unsafe Range GetIndicesRange(int start, int length, int offset = 0)
    {
        int min = int.MaxValue;
        int max = int.MinValue;

        for (int i = 0; i < length; i++)
        {
            int index = start + i;

            if (Indices[index] < min)
            {
                min = Indices[index];
            }
            if (Indices[index] > max)
            {
                max = Indices[index];
            }
        }

        return new Range(min, max);
    }

    public unsafe void ApplyOffset(int offset)
    {
        for (int i = 0; i < Indices.Length; i++)
        {
            Indices[i] += offset;
        }
    }

    //public MeshSegment CreateSectionPtr(int offset, int count)
    public MeshSegment AsSegment(int offset, int count)
    {
        return new MeshSegment(this, offset, count);
    }

    public void ApplyOffset(Vector3 offset)
    {
        for (int i = 0; i < Vertices.Length; i++)
        {
            Vertices[i].Position += offset;
        }
    }

    public void ApplyMatrix(Matrix3 matrix)
    {
        for (int i = 0; i < Vertices.Length; i++)
        {
            Vertices[i].Position *= matrix;
        }
    }
}

public enum MeshIndexType
{
    Unknown,
    UInt16,
    UInt32,
}
