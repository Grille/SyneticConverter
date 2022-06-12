using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticConverter;
public class Material
{
    public string Name;
    public MaterialType Mode;
    public bool Grass = false;
    public bool Enlite = false;
    public bool CastShadown = true;
    public TextureInfo Tex0, Tex1, Tex2;

    public Material()
    {
        Tex0 = new();
        Tex1 = new();
        Tex2 = new();
    }
}

public class TextureInfo
{
    public Texture Texture;
    public Transform Transform;
    public int Misc;
}

public enum MaterialType
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
