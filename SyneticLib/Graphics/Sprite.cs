using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;

namespace SyneticLib.Graphics;
public class Sprite
{
    public Vector2 Position;
    public Vector2 Size;
    public Texture Texture;

    public static QuadBuffer GLBuffer = new();
    public static SpriteProgram GLProgram = new();

    public Sprite(Texture texture)
    {
        Texture = texture;
    }

    public Sprite(Texture texture, Vector2 position, Vector2 size)
    {
        Position = position;
        Size = size;
        Texture = texture;
    }

    public Sprite(Texture texture, RectangleF rectangle) : this(texture, (Vector2)rectangle.Location, (Vector2)rectangle.Size) { }

    public class SpriteProgram : GLProgram
    {
        public int UScale;
        protected override void OnCreate()
        {
            Compile(GLSLSource.SpriteVertex, GLSLSource.SpriteFragment);
            UScale = GetUniformLocation("uScale");
        }
    }

    public class QuadBuffer : GLMeshBuffer
    {
        protected unsafe override void OnCreate()
        {
            ElementCount = 6;

            var indices = new ushort[6]
            {
                2,1,0,
                0,3,2,
            };
            var vertices = new Vertex[4]
            {
                new(new(-1,+1),new(0,0)),
                new(new(+1,+1),new(1,0)),
                new(new(+1,-1),new(1,1)),
                new(new(-1,-1),new(0,1)),
            };

            VerticesID = GL.GenBuffer();
            IndicesID = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndicesID);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(ushort) * indices.Length, indices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VerticesID);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(Vertex) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

            AttribID = GL.GenVertexArray();
            GL.BindVertexArray(AttribID);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(Vertex), 0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(Vertex), 8);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
        }

        record struct Vertex(Vector2 Position, Vector2 UV);
    }
}
