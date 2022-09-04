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
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.NearestMipmapLinear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, owner.Levels.Length - 1);

        int width = owner.Width;
        int height = owner.Height;
        for (int i = 0; i < owner.Levels.Length; i++)
        {
            var pixelData = owner.Levels[i].PixelData;

            if (owner.Compressed)
            {
                if (owner.Bits == 16 || owner.Bits == 24)
                {
                    GL.CompressedTexImage2D(TextureTarget.Texture2D, i, InternalFormat.CompressedRgbaS3tcDxt1Ext, width, height, 0, pixelData.Length, pixelData);
                }
                else if (owner.Bits == 32)
                {
                    GL.CompressedTexImage2D(TextureTarget.Texture2D, i, InternalFormat.CompressedRgbaS3tcDxt5Ext, width, height, 0, pixelData.Length, pixelData);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
            else
            {
                GL.TexImage2D(TextureTarget.Texture2D, i, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixelData);
            }

            width /= 2;
            height /= 2;
        }
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
