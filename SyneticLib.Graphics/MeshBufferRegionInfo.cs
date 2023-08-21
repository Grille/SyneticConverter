using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Graphics;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

public class MeshBufferRegionInfo
{
    public MeshBuffer TargetBufffer { get; }

    public int ElementOffset { get; }

    public int ElementCount { get; }

    public MeshBufferRegionInfo(MeshBuffer targetBufffer)
    {
        TargetBufffer = targetBufffer;
        ElementOffset = 0;
        ElementCount = targetBufffer.ElementCount;
    }

    public MeshBufferRegionInfo(MeshBuffer targetBufffer, int offset, int count)
    {
        TargetBufffer = targetBufffer;
        ElementOffset = offset;
        ElementCount = count;
    }

    public void DrawElements()
    {
        GL.DrawElements(PrimitiveType.Triangles, ElementCount, DrawElementsType.UnsignedInt, ElementOffset);
    }
}
