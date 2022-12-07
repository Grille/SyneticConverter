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
public unsafe class TerrainBuffer : GLMeshBuffer
{
    public readonly TerrainMesh Owner;

    public TerrainBuffer(TerrainMesh terrain)
    {
        Owner = terrain;
    }

    protected override void OnCreate()
    {
        ElementCount = Owner.Polygons.Length * 3;

        var indices = Owner.Polygons;
        var vertices = new Vertex[Owner.Vertices.Length];

        for (int i = 0; i < Owner.Vertices.Length; i++)
        {
            var srcv = Owner.Vertices[i];
            ref var dstv = ref vertices[i];

            dstv.Position = srcv.Position;
            dstv.Normal = srcv.Normal;
            dstv.UV = srcv.UV0;
            dstv.DebugColor = srcv.LightColor.ToNormalizedRGB();
        }

        VerticesID = GL.GenBuffer();
        IndicesID = GL.GenBuffer();
        AttribID = GL.GenVertexArray();

        GL.BindVertexArray(AttribID);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndicesID);
        GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(Vector3Int) * indices.Length, indices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, VerticesID);
        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(Vertex) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.LPosition);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.LNormal);
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.LUV);
        GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), Vertex.LLightColor);
        GL.EnableVertexAttribArray(0);
        GL.EnableVertexAttribArray(1);
        GL.EnableVertexAttribArray(2);
        GL.EnableVertexAttribArray(3);
    }

    [StructLayout(LayoutKind.Explicit, Size = Size)]
    internal unsafe struct Vertex
    {
        private const int s_vec2 = 4 * 2;
        private const int s_vec3 = 4 * 3;

        public const int LPosition = 0;
        public const int LNormal = LPosition + s_vec3;
        public const int LUV = LNormal + s_vec3;
        public const int LLightColor = LUV + s_vec2;
        public const int Size = LLightColor + s_vec3;

        [FieldOffset(LPosition)]
        public Vector3 Position;

        [FieldOffset(LNormal)]
        public Vector3 Normal;

        [FieldOffset(LUV)]
        public Vector2 UV;

        [FieldOffset(LLightColor)]
        public Vector3 DebugColor;
    }
}
