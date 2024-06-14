using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Files.Common;
public interface IVertexData
{
    public Vertex[] Vertecis { get; }
}

public static class IVertexDataExtension
{
    public static int GetVertexCount(this IVertexData ivtx, IIndexDataOffsets iiod)
    {
        var offsets = iiod.IndicesOffset;
        int vertexCount = 0;
        for (int i = 0; i < offsets.Length; i++)
            vertexCount += offsets[i];
        return vertexCount;
    }
}