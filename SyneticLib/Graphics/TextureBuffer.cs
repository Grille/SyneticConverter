using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

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

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, owner.Width, owner.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, owner.PixelData);
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
