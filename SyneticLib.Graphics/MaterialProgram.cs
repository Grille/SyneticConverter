using System;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using SyneticLib.Graphics.Shaders;
using SyneticLib.LowLevel;
using SyneticLib.Graphics.OpenGL;
using SyneticLib.Graphics.DrawCalls;
using System.Collections.Generic;
using SyneticLib.Math3D;

namespace SyneticLib.Graphics;
public class MaterialProgram : ShaderProgram
{
    public UniformLocation UModelMatrix4 { get; }
    public UniformLocation UViewMatrix4 { get; }
    public UniformLocation UProjectionMatrix4 { get; }

    public UniformLocation[] UTexture { get; }

    public TextureBindingInfo[] TextureBindings { get; }

    public bool IsZBufferEnabled { get; }

    public MaterialProgram(Material material, GlObjectCache<Texture, TextureBuffer>? textures, Shader shader)
    {
        IsZBufferEnabled = material.IsZBufferEnabled;

        int textureCount = material.TextureSlots.Length;

        UTexture = new UniformLocation[textureCount];
        TextureBindings = new TextureBindingInfo[textureCount];

        Link(shader);
        shader.Dispose();

        for (int i = 0; i < textureCount; i++)
        {
            TextureBindings[i] = new TextureBindingInfo(i);
            UTexture[i] = GetUniformLocation("uTexture" + i, false);
        }

        UModelMatrix4 = GetUniformLocation("uModel");
        UViewMatrix4 = GetUniformLocation("uView");
        UProjectionMatrix4 = GetUniformLocation("uProjection");
        
        Bind();

        for (int i = 0; i < textureCount; i++) {
            var uniform = UTexture[i];
            var binding = TextureBindings[i];

            if (!uniform.Enabled)
                continue;

            GL.Uniform1(uniform.Location, i);

            if (textures == null)
                continue;

            binding.TryEnable(material.TextureSlots[i], textures);
        }
    }

    public void ApplyZBuffer()
    {

    }

    public void SubCameraMatrix(Camera camera)
    {
        SubMatrix4(UViewMatrix4, camera.ViewMatrix);
        SubMatrix4(UProjectionMatrix4, camera.ProjectionMatrix);
    }

    public void SubModelMatrix(in Matrix4 matrix)
    {
        SubMatrix4(UModelMatrix4, matrix);
    }

    public void BindEnabledTextures()
    {
        for (int i = 0; i < TextureBindings.Length; i++) {
            TextureBindings[i].TryBind();
        }
    }
}
