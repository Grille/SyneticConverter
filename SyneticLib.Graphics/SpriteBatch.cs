using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

using SyneticLib.Graphics.DrawCalls;
using SyneticLib.Graphics.Materials;
using SyneticLib.Graphics.OpenGL;
using SyneticLib.Graphics.Shaders;

namespace SyneticLib.Graphics;

public class SpriteBatch : IDisposable
{
    public Vector2 ScreenSize;

    public readonly MeshBuffer QuadBuffer;
    public readonly SpriteProgram SpriteProgram;

    List<Sprite> sprites;

    public SpriteBatch()
    {
        sprites = new List<Sprite>();

        var mesh = CreateSpriteMesh();

        QuadBuffer = new MeshBuffer(mesh);
        SpriteProgram = new SpriteProgram();
    }

    public static IndexedMesh CreateSpriteMesh()
    {
        var indices = new IdxTriangleInt32[2]
        {
            new(0, 1, 2),
            new(2, 3, 0),
        };

        var vertices = new Vertex[4]
        {
            new(new(-1, +1, 0), new(0, 0)),
            new(new(+1, +1, 0), new(1, 0)),
            new(new(+1, -1, 0), new(1, 1)),
            new(new(-1, -1, 0), new(0, 1)),
        };

        return new IndexedMesh(vertices, indices);
    }

    public void Add(Sprite sprite)
    {
        sprites.Add(sprite);
    }

    public void Clear()
    {
        sprites.Clear();
    }

    public void Bind()
    {
        QuadBuffer.Bind();
        SpriteProgram.Bind();
    }

    public RectangleF ScreenToClipSpace(RectangleF rect)
    {
        return ScreenToClipSpace(new Vector2(rect.X, rect.Y), new Vector2(rect.Width, rect.Height));
    }

    public RectangleF ScreenToClipSpace(Vector2 position, Vector2 size)
    {
        var vec2Size = size / ScreenSize;
        var pos = (((position) / ScreenSize) * 2) - Vector2.One + vec2Size;
        var dst = new RectangleF(pos.X, -pos.Y, vec2Size.X, vec2Size.Y);
        return dst;
    }

    public void Draw()
    {
        var drawCall = new DrawElementsInfo(0, 2 * 3);

        for (int i = 0; i < sprites.Count; i++)
        {
            var sprite = sprites[i];
            sprite.Texture.Bind();
            SpriteProgram.SubNormalized(sprite);
            drawCall.Excecute();
        }
    }

    public void Dispose()
    {
        QuadBuffer.Dispose();
        SpriteProgram.Dispose();
        sprites = null!;
    }
}
