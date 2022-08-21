using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;

namespace SyneticLib.Graphics;
public unsafe class TerrainBuffer : GLStateObject
{
    Terrain owner;
    internal int VerticesID;
    internal int IndicesID;
    internal int AttribID;
    internal int ElementCount;

    public TerrainBuffer(Terrain terrain)
    {
        owner = terrain;
    }

    protected override void OnCreate()
    {
        ElementCount = owner.Indecies.Length;

        var indices = owner.Indecies;
        var vertices = new Vertex[owner.Vertices.Length];

        for (int i = 0; i < owner.Vertices.Length; i++)
        {
            var v = owner.Vertices[i];

            vertices[i].Position = (v.Position);
            vertices[i].DebugColor = v.LightColor.ToNormalizedRGB();
        }

        VerticesID = GL.GenBuffer();
        IndicesID = GL.GenBuffer();

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndicesID);
        GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * indices.Length, indices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, VerticesID);
        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(Vertex) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

        AttribID = GL.GenVertexArray();
        GL.BindVertexArray(AttribID);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.LocationPosition);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.LocationDebugColor);
        GL.EnableVertexAttribArray(0);
        GL.EnableVertexAttribArray(1);
    }

    protected override void OnBind()
    {
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndicesID);
        GL.BindBuffer(BufferTarget.ArrayBuffer, VerticesID);
        GL.BindVertexArray(AttribID);
    }

    protected override void OnDestroy()
    {
        GL.DeleteBuffer(IndicesID);
        GL.DeleteBuffer(VerticesID);
        GL.DeleteVertexArray(AttribID);
    }

    [StructLayout(LayoutKind.Explicit, Size = 32)]
    internal unsafe struct Vertex
    {
        public const int LocationPosition = 0;
        public const int LocationDebugColor = LocationPosition + 4 * 3;

        [FieldOffset(LocationPosition)]
        public Vector3 Position;

        [FieldOffset(LocationDebugColor)]
        public Vector3 DebugColor;
    }
}
