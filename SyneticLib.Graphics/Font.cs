using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

using SyneticLib.Graphics.OpenGL;

namespace SyneticLib.Graphics;

public class Font : IDisposable
{
    public TextureBuffer Texture { get; }

    public int Length { get; }

    public Vector2 CharUvSize { get; }

    public Font(Texture texture, int length = 256)
    {
        Texture = new TextureBuffer(texture);
        Length = length;
        CharUvSize = new Vector2(1f, 1f / Length);
    }

    public Font(TextureBuffer texture, int length = 256)
    {
        Texture = texture;
        Length = length;
        CharUvSize = new Vector2(1f, 1f / Length);
    }

    public RectangleF GetUV(char c)
    {
        int index = Math.Clamp(c, 0, Length);
        float offsetY = index / (float)Length;

        return new RectangleF(0, offsetY, CharUvSize.X, CharUvSize.Y);
    }

    public void Bind()
    {
        Texture.Bind();
    }

    public void Dispose()
    {
        Texture.Dispose();
    }
}
