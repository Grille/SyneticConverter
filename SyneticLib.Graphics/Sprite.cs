using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using SyneticLib.Graphics.Shaders;
using System.Runtime.CompilerServices;
using SyneticLib.Graphics.OpenGL;

namespace SyneticLib.Graphics;
public class Sprite
{
    public Vector2 Offset { get; }
    public Vector2 Scale { get; }

    public TextureBuffer Texture { get; }

    private Sprite(TextureBuffer texture)
    {
        Texture = texture;
    }

    public Sprite(TextureBuffer texture, Vector2 offset, Vector2 size) : this(texture)
    {
        Offset = offset;
        Scale = size;
    }

    public Sprite(TextureBuffer texture, RectangleF rectangle) : this(texture) {
        Offset = new Vector2(rectangle.X, rectangle.Y);
        Scale = new Vector2(rectangle.Width, rectangle.Height);
    }
}
