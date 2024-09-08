using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;
using System.Runtime.InteropServices;

namespace SyneticLib;

public class Mesh : SyneticObject
{
    public Vertex[] Vertices { get; set; }
    public IdxTriangleInt32[] Indices { get; set; }

    public MeshIndicesType IndicesType { get; private set; }

    public Mesh(Vertex[] vertices, IdxTriangleInt32[] indices, MeshIndicesType type = MeshIndicesType.UInt32)
    {
        Vertices = vertices;
        Indices = indices;

        if (type == MeshIndicesType.Unknown)
        {
            UpdateIndicesType();
        }
        else
        {
            IndicesType = type;
        }
    }

    public void UpdateIndicesType()
    {
        var range = GetIndicesRange();
        IndicesType = range.End.Value > ushort.MaxValue ? MeshIndicesType.UInt32 : MeshIndicesType.UInt16;
    }

    public unsafe Range GetIndicesRange()
    {
        int length = Indices.Length * 3;
        int min = int.MaxValue;
        int max = int.MinValue;
        fixed (void* vptr = Indices)
        {
            var ptr = (int*)vptr;
            for (int i = 0; i < length; i++)
            {
                if (ptr[i] < min)
                {
                    min = ptr[i];
                }
                if (ptr[i] > max)
                {
                    max = ptr[i];
                }
            }
        }
        return new Range(min, max);
    }

    public unsafe void ApplyOffset(int offset)
    {
        int length = Indices.Length * 3;
        fixed (void* vptr = Indices)
        {
            var ptr = (int*)vptr;
            for (int i = 0; i < length; i++)
            {
                ptr[i] += offset;
            }
        }
    }

    //public MeshSegment CreateSectionPtr(int offset, int count)
    public MeshSegment AsSegment(int offset, int count)
    {
        return new MeshSegment(this, offset, count);
    }
}

public enum MeshIndicesType
{
    Unknown,
    UInt16,
    UInt32,
}
