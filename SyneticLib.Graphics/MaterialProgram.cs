using System;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using SyneticLib.Graphics.Shaders;
using SyneticLib.LowLevel;
using SyneticLib.Graphics.OpenGL;
using SyneticLib.Graphics.DrawCalls;
using System.Collections.Generic;

namespace SyneticLib.Graphics;
public class MaterialProgram : ShaderProgram
{
    public int[] UTexture { get; }

    public int UEffectTexture0 { get; }

    public int UEffectTexture1 { get; }

    public int UCubeTexture { get; }

    public int UShaderType { get; }

    public int UColorAmbient {get; protected set;}
    public int UColorDiffuse { get; protected set; }
    public int UColorSpec1 { get; protected set; }
    public int UColorSpec2 { get; protected set; }
    public int UColorReflect { get; protected set; }

    public TextureBindingInfo[] TextureBindings { get; }

    public MaterialProgram(Material material, GlObjectCache<Texture, TextureBuffer>? textures = null)
    {
        int textureCount = material.TextureSlots.Length;

        UTexture = new int[textureCount];
        TextureBindings = new TextureBindingInfo[textureCount];

        Link(material.ShaderType);

        for (int i = 0; i < textureCount; i++)
        {
            TextureBindings[i] = new TextureBindingInfo(i);
            UTexture[i] = GetUniformLocation("uTexture" + i);
        }

        UModelMatrix4 = GetUniformLocation("uModel");
        UViewMatrix4 = GetUniformLocation("uView");
        UProjectionMatrix4 = GetUniformLocation("uProjection");

        UShaderType = GetUniformLocation("uShaderType");
        UColorAmbient = GetUniformLocation("uColorAmbient");
        UColorDiffuse = GetUniformLocation("uColorDiffuse");

        
        Bind();

        SubSingle(UShaderType, (int)material.ShaderType);
        SubVector3(UColorDiffuse, material.Diffuse);

        for (int i = 0; i < textureCount; i++) {
            var uniform = UTexture[i];
            var binding = TextureBindings[i];

            if (uniform == -1)
                continue;

            GL.Uniform1(uniform, i);

            if (textures == null)
                continue;

            binding.TryEnable(material.TextureSlots[i], textures);
        }
    }

    public void BindEnabledTextures()
    {
        for (int i = 0; i < TextureBindings.Length; i++) {
            TextureBindings[i].TryBind();
        }
    }
}
