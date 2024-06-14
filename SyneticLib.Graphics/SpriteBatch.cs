using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Graphics.DrawCalls;
using SyneticLib.Graphics.OpenGL;

namespace SyneticLib.Graphics;

public class SpriteBatch : IDisposable
{
    MeshBuffer quadBuffer;
    SpriteProgram spriteProgram;

    List<Sprite> sprites;

    public SpriteBatch()
    {
        sprites = new List<Sprite>();

        var mesh = CreateSpriteMesh();

        quadBuffer = new MeshBuffer(mesh);
        spriteProgram = new SpriteProgram();  
    }

    public static Mesh CreateSpriteMesh()
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

        return new Mesh(vertices, indices);
    }

    public void Add(Sprite sprite)
    {
        sprites.Add(sprite);
    }

    public void Clear()
    {
        sprites.Clear();
    }

    public void Draw()
    {
        quadBuffer.Bind();
        spriteProgram.Bind();

        var drawCall = new DrawElementsInfo(0, 2*3);

        for (int i = 0; i < sprites.Count; i++) { 
            var sprite = sprites[i];
            sprite.Texture.Bind();
            spriteProgram.Sub(sprite);
            drawCall.Excecute();
        }
    }

    public void Dispose()
    {
        quadBuffer.Dispose();
        spriteProgram.Dispose();
        sprites = null!;
    }
}
