using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

using SyneticLib.Graphics.DrawCalls;
using SyneticLib.Graphics.OpenGL;
using SyneticLib.Graphics.Shaders;

namespace SyneticLib.Graphics;

public class TextBatch : IDisposable
{
    public Vector2 ScreenSize => _sprites.ScreenSize;

    public readonly MeshBuffer QuadBuffer;
    public readonly SpriteProgram TextProgram;

    readonly SpriteBatch _sprites;

    public TextBatch(SpriteBatch spriteBatch)
    {
        _sprites = spriteBatch;

        QuadBuffer = spriteBatch.QuadBuffer;
        TextProgram = new SpriteProgram(GLSLSources.Text);
    }

    public void Bind()
    {
        QuadBuffer.Bind();
        TextProgram.Bind();
    }

    public void UseColor(Color4 color)
    {
        TextProgram.SubColor((Vector4)color);
    }

    public void Draw(ReadOnlySpan<char> text, Vector2 point, float size)
    {
        var drawCall = new DrawElementsInfo(0, 2 * 3);

        var vec2Size = new Vector2(size * 1.14f, size);

        float charHeight = 1 / 224f;

        for (int i = 0; i < text.Length; i++)
        {
            var position = point + new Vector2(i * size * 0.5f, 0);
            var dst = _sprites.ScreenToClipSpace(position, vec2Size);

            float charPos = (text[i] - 0x20) / 224f;
            var src = new RectangleF(0, charPos, 1, charHeight);

            TextProgram.SubNormalized(dst, src);
            drawCall.Excecute();
        }
    }

    public void Dispose()
    {
        TextProgram.Dispose();
    }
}
