﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

namespace SyneticLib;
public struct BoundingBox
{
    public Vector3 Start;
    public Vector3 End;

    public Vector3 Size => End - Start;

    public BoundingBox(Vector3 position)
    {
        Init(position);
    }

    public BoundingBox(Vector3 start, Vector3 end)
    {
        Start = start;
        End = end;
    }

    public BoundingBox(ReadOnlySpan<Vertex> vertices)
    {
        if (vertices.Length == 0)
            throw new ArgumentException(nameof(vertices));

        Init(vertices[0].Position);

        for (int i = 0; i < vertices.Length; i++)
        {
            Extend(vertices[i].Position);
        }
    }

    public BoundingBox(ReadOnlySpan<Vertex> vertices, ReadOnlySpan<IdxTriangleInt32> indices, int offset = 0)
    {
        if (vertices.Length == 0)
            throw new ArgumentException(null, nameof(vertices));

        Init(vertices[indices[0].X].Position);

        for (int i = 0; i < indices.Length; i++)
        {
            Extend(vertices[indices[i].X + offset].Position);
            Extend(vertices[indices[i].Y + offset].Position);
            Extend(vertices[indices[i].Z + offset].Position);
        }
    }

    public void Init(Vector3 position)
    {
        Start = position;
        End = position;
    }

    public void Extend(BoundingBox boundings)
    {
        Start = Vector3.ComponentMin(Start, boundings.Start);
        End = Vector3.ComponentMax(End, boundings.End);
    }

    public void Extend(Vector3 position)
    {
        Start = Vector3.ComponentMin(Start, position);
        End = Vector3.ComponentMax(End, position);
    }

    public override string ToString()
    {
        return $"{Start} {End}";
    }
}
