using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;
using static OpenTK.Graphics.OpenGL.GL;
using SyneticLib.LowLevel;

namespace SyneticLib.Graphics;
public unsafe sealed class MeshBuffer : GLObject
{
    public int VerticesID { get; }
    public int IndicesID { get; }
    public int VertexArrayID { get; }
    public int ElementCount { get; }
    public int VertexStride { get; }

    public MeshBuffer(Context ctx, Mesh mesh) : base(ctx)
    {
        VertexArrayID = GL.GenVertexArray();
        VerticesID = GL.GenBuffer();
        IndicesID = GL.GenBuffer();

        ElementCount = mesh.Indices.Length * 3;
        VertexStride = sizeof(Vertex);

        var indices = mesh.Indices;
        var vertices = mesh.Vertices;

        GL.BindVertexArray(VertexArrayID);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndicesID);
        GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(IndexTriangle) * indices.Length, indices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, VerticesID);
        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(Vertex) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.Layout.Position);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.Layout.Normal);
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.Layout.UV0);
        GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.Layout.LightColor);
        GL.EnableVertexAttribArray(0);
        GL.EnableVertexAttribArray(1);
        GL.EnableVertexAttribArray(2);
        GL.EnableVertexAttribArray(3);
    }

    protected sealed override void OnBind()
    {
        GL.BindVertexArray(VertexArrayID);
    }

    protected sealed override void OnDelete()
    {
        GL.DeleteVertexArray(VertexArrayID);
        GL.DeleteBuffer(IndicesID);
        GL.DeleteBuffer(VerticesID);
    }
}
