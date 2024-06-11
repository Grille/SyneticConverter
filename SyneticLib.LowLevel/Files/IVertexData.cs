using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Files;
public interface IVertexData
{
    public int[] IndicesOffset { get; }
    public Vertex[] Vertecis { get; }
}

public static class IVertexDataExtension
{
    public static int GetVertexCount(this IVertexData ivtx )
    {
        var offsets = ivtx.IndicesOffset;
        int vertexCount = 0;
        for (int i = 0; i < offsets.Length; i++)
            vertexCount += offsets[i];
        return vertexCount;
    }
}