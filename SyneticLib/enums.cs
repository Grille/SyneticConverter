﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SyneticLib;

namespace SyneticLib;

public enum TextureFormat
{
    Unknown,
    R8,
    Bgr24,
    Rgb24,
    Bgra32,
    Rgba32,
    Rgb24Dxt1,
    Rgba32Dxt5,
}

public enum SoundFormat
{
    Wav
}

public static class TextureFormatExtension
{
    public static int Stride(this TextureFormat format) => format switch
    {
        _ => throw new NotImplementedException(),
    };

    public static int BitSize(this TextureFormat format) => format switch
    {
        TextureFormat.R8 => 8,
        TextureFormat.Bgr24 => 24,
        TextureFormat.Rgb24 => 24,
        TextureFormat.Bgra32 => 32,
        TextureFormat.Rgba32 => 32,
        _ => throw new NotSupportedException(),
    };

    public static int ByteSize(this TextureFormat format) => BitSize(format) / 8;
}

public enum MaterialShadingMode
{
    Tex3Terrain = 0,
    Tex3UTT = 16,
    Tex1Uv = 32,
    Tex2UvDiffBlend0 = 48,
    Tex2UvDiffBlend1 = 64,
    Tex2UvDiffAbsBlend = 80,
    Tex2UvDiffBlend2 = 96,
    Tex1UvReflective = 112,
    Tex2HUvSpec = 144,
    Tex2UvSpecAbsAlpha = 160,
    Tex2UvSpecAlpha = 176,
    Tex1UvSpec = 192,
    Tex1Masked = 208,
    Tex3Water = 224,
}
