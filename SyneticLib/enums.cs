using System;
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
    RGB24,
    BGRA32,
    RGBA32,
    RGB24Dxt1,
    RGBA32Dxt5,
}

public static class TextureFormatExtension
{
    public static int Stride(this TextureFormat format) => format switch
    {
        _ => throw new NotImplementedException(),
    };
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
