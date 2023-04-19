using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;

namespace SyneticLib.Graphics;
public abstract class GLMeshBuffer : GLStateObject
{
    public int VerticesID;
    public int IndicesID;
    public int VertexArrayID;
    public int ElementCount;

    public bool IsInitialized;


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
