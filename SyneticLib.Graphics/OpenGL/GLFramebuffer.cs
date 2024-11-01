using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

using SyneticLib.Math3D;

namespace SyneticLib.Graphics.OpenGL;

public class GLFramebuffer : GLObject
{
    public int Width { get; }
    public int Height { get; }
    public int BufferID { get; }

    public TextureBuffer Color0 { get; }
    public TextureBuffer Color1 { get; }
    public TextureBuffer Color2 { get; }
    public TextureBuffer Color3 { get; }
    public TextureBuffer DepthStencil { get; }

    public GLFramebuffer(int width, int height)
    {
        Width = width;
        Height = height;

        BufferID = GL.GenFramebuffer();

        Color0 = new TextureBuffer(width, height, PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.UnsignedByte);
        Color1 = new TextureBuffer(width, height, PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.UnsignedByte);
        Color2 = new TextureBuffer(width, height, PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.UnsignedByte);
        Color3 = new TextureBuffer(width, height, PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.UnsignedByte);
        DepthStencil = new TextureBuffer(width, height, PixelInternalFormat.Depth24Stencil8, PixelFormat.DepthStencil, PixelType.UnsignedInt248);

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, BufferID);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, Color0.TextureID, 0);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2D, Color1.TextureID, 0);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment2, TextureTarget.Texture2D, Color2.TextureID, 0);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment3, TextureTarget.Texture2D, Color3.TextureID, 0);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2D, DepthStencil.TextureID, 0);

        GL.DrawBuffers(4, new[] { DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment1, DrawBuffersEnum.ColorAttachment2, DrawBuffersEnum.ColorAttachment3 });
    }

    public static void BindDefault()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    public void Viewport()
    {
        GL.Viewport(0, 0, Width, Height);
    }

    protected override void OnBind()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, BufferID);
    }

    protected override void OnDelete()
    {
        GL.DeleteFramebuffer(BufferID);

        Color0.Dispose();
        Color1.Dispose();
        Color2.Dispose();
        Color3.Dispose();
        DepthStencil.Dispose();
    }
}
