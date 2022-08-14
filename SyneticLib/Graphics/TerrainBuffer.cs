using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

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
        var vertices = new GlTerrainVertex[owner.Vertices.Length];

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
        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(GlTerrainVertex) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

        AttribID = GL.GenVertexArray();
        GL.BindVertexArray(AttribID);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(GlTerrainVertex), GlTerrainVertex.LocationPosition);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(GlTerrainVertex), GlTerrainVertex.LocationDebugColor);
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
}
