using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace SyneticLib.Graphics;
public class TextureBuffer : GLStateObject
{
    Texture owner;
    int textureID;

    public TextureBuffer(Texture texture)
    {
        owner = texture;
    }

    protected override void OnCreate()
    {
        textureID = GL.GenTexture();

        GL.BindTexture(TextureTarget.Texture2D, textureID);

        //GL.texs
        //GL.texsub

    }

    protected override void OnBind()
    {
        GL.BindTexture(TextureTarget.Texture2D, textureID);
    }

    protected override void OnDestroy()
    {
        GL.DeleteTexture(textureID);
    }
}
