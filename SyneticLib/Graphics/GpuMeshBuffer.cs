using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;
using OpenTK.Graphics.OpenGL4;

namespace SyneticLib.Graphics;
public unsafe class GpuMeshBuffer : IDisposable
{
    internal Mesh Mesh;
    internal int VerticesID;
    internal int IndicesID;
    internal int AttribID;
    internal int ElementCount;

    public bool IsInitialized;

    internal GpuMeshBuffer(Mesh mesh)
    {
        Mesh = mesh;
    }

    public void Initialize()
    {
        ElementCount = Mesh.Indecies.Length;

        var indices = Mesh.Indecies;
        var vertices = new GpuTerrainVertex[Mesh.Vertices.Length];

        for (int i = 0; i < Mesh.Vertices.Length; i++)
        {
            var v = Mesh.Vertices[i];

            vertices[i].Position = (v.Position);
            vertices[i].DebugColor = v.LightColor.ToNormalizedRGB();
        }

        VerticesID = GL.GenBuffer();
        IndicesID = GL.GenBuffer();

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndicesID);
        GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * indices.Length, indices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, VerticesID);
        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(GpuTerrainVertex) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

        AttribID = GL.GenVertexArray();
        GL.BindVertexArray(AttribID);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(GpuTerrainVertex), GpuTerrainVertex.LocationPosition);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(GpuTerrainVertex), GpuTerrainVertex.LocationDebugColor);
        GL.EnableVertexAttribArray(0);
        GL.EnableVertexAttribArray(1);

        IsInitialized = true;
    }

    public void Bind()
    {
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndicesID);
        GL.BindBuffer(BufferTarget.ArrayBuffer, VerticesID);
        GL.BindVertexArray(AttribID);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (IsInitialized)
        {
            GL.DeleteBuffer(IndicesID);
            GL.DeleteBuffer(VerticesID);
            GL.DeleteVertexArray(AttribID);

            IsInitialized = false;
        }
    }

    ~GpuMeshBuffer()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
