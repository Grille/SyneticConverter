using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using OpenTK.Mathematics;
using static OpenTK.Graphics.OpenGL.GL;
using OpenTK.Compute.OpenCL;
using SyneticLib.Graphics.OpenGL;
using System.Diagnostics.CodeAnalysis;

namespace SyneticLib.Graphics.DrawCalls;

public class TextureBindingInfo
{
    public int Sampler { get; }

    public TextureBuffer? Texture { get; private set; }

    [MemberNotNullWhen(true, nameof(Texture))]
    public bool Enabled { get; private set; }

    public TextureBindingInfo(int sampler)
    {
        Sampler = sampler;
    }

    public void TryEnable(Material.TextureSlot slot, GlObjectCache<Texture, TextureBuffer> cache)
    {
        if (!slot.Enabled)
            return;

        var buffer = cache.GetGlObject(slot.Texture!);
        Enable(buffer);
    }

    public void Enable(TextureBuffer buffer)
    {
        Texture = buffer;
        Enabled = true;
    }

    public bool TryBind()
    {
        if (!Enabled)
            return false;

        Texture.Bind(Sampler);
        return true;
    }

    public void Bind()
    {
        if (!Enabled)
            return;

        Texture.Bind(Sampler);
    }
}
