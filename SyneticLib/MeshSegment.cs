﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class MeshSegment
{
    public Mesh Mesh { get; }

    public int Offset { get; }
    public int Start { get; }
    public int Length { get; }

    public int End => Start + Length;

    public BoundingBox BoundingBox;

    public Span<Vertex> Vertices => new Span<Vertex>(Mesh.Vertices);

    public Span<IdxTriangleInt32> Indices => new Span<IdxTriangleInt32>(Mesh.Indices, Start, Length);

    public MeshSegment(Mesh mesh)
    {
        Mesh = mesh;
        Offset = 0;
        Start = 0;
        Length = mesh.Indices.Length;

        BoundingBox = new BoundingBox(Vertices, Indices, 0);
    }

    public MeshSegment(Mesh mesh, int start, int length, int offset = 0)
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

        BoundingBox = new BoundingBox(Vertices, Indices, offset);
    }

    public void UpdateBoundingBox()
    {
        BoundingBox = new BoundingBox(Vertices, Indices, Offset);
    }

    public Mesh ToMesh()
    {
        var vertecies = Vertices.ToArray();
        var indices = Indices.ToArray();

        return new Mesh(vertecies, indices);
    }
}
