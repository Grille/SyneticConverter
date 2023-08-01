using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;

namespace SyneticLib.Graphics;
public abstract class GLMeshBuffer : GLStateObject
{
    public int VerticesID { get; }
    public int IndicesID { get; }
    public int VertexArrayID { get; }
    public int ElementCount { get; protected set; }
    public int VertexStride { get; protected set; }

    public GLMeshBuffer()
    {
        VertexArrayID = GL.GenVertexArray();
        VerticesID = GL.GenBuffer();
        IndicesID = GL.GenBuffer();
    }

    protected sealed override void OnBind()
    {
        GL.BindVertexArray(VertexArrayID);
    }

    protected sealed override void OnDestroy()
    {
        GL.DeleteBuffer(IndicesID);
        GL.DeleteBuffer(VerticesID);
        GL.DeleteVertexArray(VertexArrayID);
    }
}
