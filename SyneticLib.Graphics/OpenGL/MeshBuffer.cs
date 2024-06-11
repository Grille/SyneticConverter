using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;
using static OpenTK.Graphics.OpenGL.GL;

namespace SyneticLib.Graphics.OpenGL;
public unsafe sealed class MeshBuffer : GLObject
{
    public int VerticesID { get; }
    public int IndicesID { get; }
    public int VertexArrayID { get; }
    public int ElementCount { get; }
    public int VertexStride { get; }

    public MeshBuffer(Mesh mesh)
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
        GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(IdxTriangleInt32) * indices.Length, indices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, VerticesID);
        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(Vertex) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.Layout.Position);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.Layout.Normal);
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.Layout.UV0);
        GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.Layout.UV1);
        GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.Layout.Blending);
        GL.VertexAttribPointer(5, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.Layout.LightColor);
        GL.VertexAttribPointer(6, 1, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.Layout.Shadow);
        GL.EnableVertexAttribArray(0);
        GL.EnableVertexAttribArray(1);
        GL.EnableVertexAttribArray(2);
        GL.EnableVertexAttribArray(3);
        GL.EnableVertexAttribArray(4);
        GL.EnableVertexAttribArray(5);
        GL.EnableVertexAttribArray(6);
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
