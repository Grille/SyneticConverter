using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Graphics.Shaders;

using SyneticLib.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;
using System.Drawing;

namespace SyneticLib.Graphics.Materials;

public class SpriteProgram : ShaderProgram
{
    public UniformLocation UDst { get; protected set; }
    public UniformLocation USrc { get; protected set; }

    public UniformLocation UColor { get; protected set; }

    public SpriteProgram() : this(GLSLSources.Sprite) { }

    public SpriteProgram(GlslFragmentShaderSource frag)
    {
        Link(GLSLSources.VSprite, frag);

        UDst = GetUniformLocation("uDst");
        USrc = GetUniformLocation("uSrc");
        UColor = GetUniformLocation("uColor");
    }

    public void SubNormalized(Sprite sprite)
    {
        SubVector4(UDst, ToVector4(sprite.Dst));
        SubVector4(UDst, ToVector4(sprite.Src));
        SubVector4(UDst, sprite.Color);
    }

    public void SubNormalized(RectangleF dst, RectangleF src)
    {
        SubVector4(UDst, ToVector4(dst));
        SubVector4(USrc, ToVector4(src));
    }

    public void SubColor(Vector4 color)
    {
        SubVector4(UColor, color);
    }

    public void Sub(Sprite sprite, Vector2 screenSize)
    {

    }

    static Vector4 ToVector4(RectangleF rect)
    {
        return Unsafe.As<RectangleF, Vector4>(ref rect);
    }
}
