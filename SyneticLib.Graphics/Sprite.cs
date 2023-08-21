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

namespace SyneticLib.Graphics;
public class Sprite
{
    public Vector2 Position { get; }
    public Vector2 Size { get; }

    public Texture Texture { get; }

    public static MeshBuffer GLBuffer;
    public static SpriteProgram GLProgram = new();

    private Sprite(Texture texture)
    {
        Texture = texture;
    }

    public Sprite(Texture texture, Vector2 position, Vector2 size) : this(texture)
    {
        Position = position;
        Size = size;
    }

    public Sprite(Texture texture, RectangleF rectangle) : this(texture) {
        Position = new Vector2(rectangle.X, rectangle.Y);
        Size = new Vector2(rectangle.Width, rectangle.Height);
    }

    public class SpriteProgram : GLProgram
    {
        public int UScale;
        public SpriteProgram() : base(null)
        {
            Compile(GLSLSources.SpriteVertex, GLSLSources.SpriteFragment);
            UScale = GetUniformLocation("uScale");
        }
    }
}
