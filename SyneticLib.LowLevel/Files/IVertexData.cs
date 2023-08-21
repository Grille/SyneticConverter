using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.LowLevel.Files;
public interface IVertexData
{
    public int[] IndicesOffset { get; set; }
    public Vertex[] Vertecis { get; set; }

    public int GetVertexCount()
    {
        int vertexCount = 0;
        for (int i = 0; i < IndicesOffset.Length; i++)
            vertexCount += IndicesOffset[i];
        return vertexCount;
    }
}