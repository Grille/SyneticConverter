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
            var srcv = owner.Vertices[i];
            ref var dstv = ref vertices[i];

            dstv.Position = srcv.Position;
            dstv.Normal = srcv.Normal;
            dstv.UV = srcv.UV0;
            dstv.DebugColor = srcv.LightColor.ToNormalizedRGB();
        }

        VerticesID = GL.GenBuffer();
        IndicesID = GL.GenBuffer();

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndicesID);
        GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * indices.Length, indices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, VerticesID);
        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(Vertex) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

        AttribID = GL.GenVertexArray();
        GL.BindVertexArray(AttribID);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.LPosition);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.LNormal);
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.LUV);
        GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.LDebugColor);
        GL.EnableVertexAttribArray(0);
        GL.EnableVertexAttribArray(1);
        GL.EnableVertexAttribArray(2);
        GL.EnableVertexAttribArray(3);
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

    [StructLayout(LayoutKind.Explicit, Size = Size)]
    internal unsafe struct Vertex
    {
        private const int s_vec2 = 4 * 2;
        private const int s_vec3 = 4 * 3;

        public const int LPosition = 0;
        public const int LNormal = LPosition + s_vec3;
        public const int LUV = LNormal + s_vec3;
        public const int LDebugColor = LUV + s_vec2;
        public const int Size = LDebugColor + s_vec3;

        [FieldOffset(LPosition)]
        public Vector3 Position;

        [FieldOffset(LNormal)]
        public Vector3 Normal;

        [FieldOffset(LUV)]
        public Vector2 UV;

        [FieldOffset(LDebugColor)]
        public Vector3 DebugColor;
    }
}
