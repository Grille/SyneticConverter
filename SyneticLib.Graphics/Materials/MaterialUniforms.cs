using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Graphics.DrawCalls;
using SyneticLib.Graphics.OpenGL;

namespace SyneticLib.Graphics.Materials;

public abstract class MaterialUniforms : GLObject
{
    public bool IsZBufferEnabled { get; }

    public TextureBindingInfo[] TextureBindings { get; }

    public MaterialUniforms(Material material, GlObjectCache<Texture, TextureBuffer>? textures)
    {
        IsZBufferEnabled = material.IsZBufferEnabled;

        int textureCount = material.TextureSlots.Length;

        TextureBindings = new TextureBindingInfo[textureCount];


        for (int i = 0; i < textureCount; i++)
        {
            var binding = new TextureBindingInfo(i);
            TextureBindings[i] = binding;

            if (textures == null)
                continue;

            binding.TryEnable(material.TextureSlots[i], textures);
        }
    }

    public void BindEnabledTextures()
    {
        for (int i = 0; i < TextureBindings.Length; i++)
        {
            TextureBindings[i].TryBind();
        }
    }
}
