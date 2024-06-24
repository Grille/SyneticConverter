using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace SyneticLib.Graphics.OpenGL;
public sealed class TextureBuffer : GLObject
{
    public int TextureID { get; }

    public TextureBuffer(int width, int height, PixelInternalFormat internalfomrat, PixelFormat format, PixelType pixelType)
    {
        TextureID = GL.GenTexture();

        GL.BindTexture(TextureTarget.Texture2D, TextureID);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        GL.TexImage2D(TextureTarget.Texture2D, 0, internalfomrat, width, height, 0, format, pixelType, (IntPtr)null);
    }

    public TextureBuffer(Texture texture)
    {
        TextureID = GL.GenTexture();

        GL.BindTexture(TextureTarget.Texture2D, TextureID);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, texture.Levels.Length - 1);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxAnisotropy, 16);

        int width = texture.Width;
        int height = texture.Height;
        for (int i = 0; i < texture.Levels.Length; i++)
        {
            var pixelData = texture.Levels[i].Data;


            switch (texture.Format)
            {
                case TextureFormat.RGB24Dxt1:
                {
                    GL.CompressedTexImage2D(TextureTarget.Texture2D, i, InternalFormat.CompressedRgbaS3tcDxt1Ext, width, height, 0, pixelData.Length, pixelData);
                    break;
                }
                case TextureFormat.RGBA32Dxt5:
                {
                    GL.CompressedTexImage2D(TextureTarget.Texture2D, i, InternalFormat.CompressedRgbaS3tcDxt5Ext, width, height, 0, pixelData.Length, pixelData);
                    break;
                }
                case TextureFormat.BGRA32:
                {
                    GL.TexImage2D(TextureTarget.Texture2D, i, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, pixelData);
                    break;
                }
                case TextureFormat.RGBA32:
                {
                    GL.TexImage2D(TextureTarget.Texture2D, i, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixelData);
                    break;
                }
                case TextureFormat.RGB24:
                {
                    GL.TexImage2D(TextureTarget.Texture2D, i, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, pixelData);
                    break;
                }
                case TextureFormat.R8:
                {
                    GL.TexImage2D(TextureTarget.Texture2D, i, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Red, PixelType.UnsignedByte, pixelData);
                    break;
                }
                default:
                {
                    throw new NotSupportedException();
                }
            }

            width /= 2;
            height /= 2;
        }
    }

    public void Bind(int sampler)
    {
        var textureUnit = TextureUnit.Texture0 + sampler;
        GL.ActiveTexture(textureUnit);
        Bind();
    }

    protected override void OnBind()
    {
        GL.BindTexture(TextureTarget.Texture2D, TextureID);
    }

    protected override void OnDelete()
    {
        GL.DeleteTexture(TextureID);
    }
}
