using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace SyneticLib.Graphics;
public class Sprite
{
    public Vector2 Position;
    public Vector2 Size;
    public Texture Texture;

    public static GLProgram GLProgram = new SpriteProgram();

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

    class SpriteProgram : GLProgram
    {
        protected override void OnCreate()
        {
            Compile(GLSLSource.SpriteVertex, GLSLSource.SpriteFragment);
        }
    }
}
