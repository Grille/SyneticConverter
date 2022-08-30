using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;

namespace SyneticLib.Graphics;
public unsafe class ModelBuffer : GLMeshBuffer
{
    Model Owner;
    internal ModelBuffer(Model mesh)
    {
        Owner = mesh;
    }

    protected override void OnCreate()
    {
        ElementCount = Owner.Poligons.Length * 3;

        var indices = Owner.Poligons;
        var vertices = new Vertex[Owner.Vertices.Length];

        for (int i = 0; i < Owner.Vertices.Length; i++)
        {
            var v = Owner.Vertices[i];

            vertices[i].Position = (v.Position);
            vertices[i].DebugColor = v.LightColor.ToNormalizedRGB();
        }

        VerticesID = GL.GenBuffer();
        IndicesID = GL.GenBuffer();

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndicesID);
        GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * 3 * indices.Length, indices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, VerticesID);
        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(Vertex) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

        AttribID = GL.GenVertexArray();
        GL.BindVertexArray(AttribID);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.LocationPosition);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.LocationDebugColor);
        GL.EnableVertexAttribArray(0);
        GL.EnableVertexAttribArray(1);

        IsInitialized = true;
    }

    [StructLayout(LayoutKind.Explicit, Size = Size)]
    public struct Vertex
    {
        public const int LocationPosition = 0;
        public const int LocationDebugColor = LocationPosition + 4 * 3;
        public const int Size = LocationDebugColor + 4 * 3;

        [FieldOffset(LocationPosition)]
        public Vector3 Position;

        [FieldOffset(LocationDebugColor)]
        public Vector3 DebugColor;
    }
}
