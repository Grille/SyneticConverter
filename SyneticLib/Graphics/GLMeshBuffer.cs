using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;

namespace SyneticLib.Graphics;
public abstract class GLMeshBuffer : GLStateObject
{
    internal int VerticesID;
    internal int IndicesID;
    internal int AttribID;
    internal int ElementCount;

    public bool IsInitialized;


    protected sealed override void OnBind()
    {
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndicesID);
        GL.BindBuffer(BufferTarget.ArrayBuffer, VerticesID);
        GL.BindVertexArray(AttribID);
    }

    protected sealed override void OnDestroy()
    {
        GL.DeleteBuffer(IndicesID);
        GL.DeleteBuffer(VerticesID);
        GL.DeleteVertexArray(AttribID);
    }
}
