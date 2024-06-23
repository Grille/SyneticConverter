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
    public readonly RectangleF Dst;
    public readonly RectangleF Src;
    public readonly Vector4 Color;
    public readonly bool IsDstNormalized;

    public TextureBuffer Texture { get; }

    public static readonly RectangleF DefaultSrc = new RectangleF(0, 0, 1, 1);

    private Sprite(TextureBuffer texture)
    {
        Texture = texture;
    }

    public Sprite(TextureBuffer texture, RectangleF dst, RectangleF src, Vector4 color, bool normalized = true) : this(texture)
    {
        Dst = dst;
        Src = src;
        Color = color;
        IsDstNormalized = normalized;
    }

    public Sprite(TextureBuffer texture, RectangleF dst, bool normalized = true) : this(texture) {
        Dst = dst;
        Src = DefaultSrc;
        Color = Vector4.One;
        IsDstNormalized = normalized;
    }
}
