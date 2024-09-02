using SyneticLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;

public enum TerrainMaterialTypeWR2 : ushort
{
    Terrain = 0,
    UVTerrain = 16,
    UV = 32,
    Reflective = 112,
    Road0 = 128,
    Road1 = 144,
    Road2 = 160,
    Road3 = 176,
    AlphaClip = 208,
    Water = 224,
    AlphaBlend = 240,
}

public enum TerrainMaterialTypeMBWR : ushort
{
    Terrain = 0,
    UVTerrain = 16,
    UV = 32,
    Road0 = 48,
    Road1 = 80,
    Road2 = 112,
    Road3 = 96,
    Reflective = 64,
    AlphaClip = 224,
    Water = 208,
    AlphaBlend = 240,
}

public enum TerrainMaterialTypeC11 : ushort
{
    Terrain = 0,
    UVTerrain = 16,
    UV = 32,
    Road0 = 240,
    Reflective = 192,
    AlphaClip = 288,
    Water = 320,
    AlphaBlend = 240,
}

public enum _TerrainMaterialTypeWR2 : ushort
{
    Terrain,
    Road,
    _1Layer_refl,
    _2Layer_refl1,
    _2Layer_refl2,
    _2Layer_refl_ovl_t,
    _2Layer_refl_ovl,
    Windows,
    _1Layer_spec,
    _2Layer_spec_faded,
    _2Layer_spec_overlay_t,
    _2Layer_spec_overlay,
    None0,
    Colorkey,
    Water,
    Alpha,
}
/*
public enum _TerrainMaterialTypeC11 : ushort
{
    _4L_diff L 1,2,3,4
    _1L_diff
    _1L_refl
    _1L_metal
    _1L_spec
    _2L_diff diff L 1,2
    _2L_diff diff L 1,3
    _2L_refl diff L 1,2
    _2L_refl diff L 1,3
    _2L_metal diff L 1,2
    _2L_spec spec L 1,2
    _2L_spec diff L 1,3
    _2L_refl diff win L 1,2
    _3L_diff diff diff
    _3L_refl diff diff
    _3L_refl refl diff
    _3L_spec spec diff
    _3L_spec diff diff
    _2L_diff diff L 1,2 ColKey L 1
    _2L_refl diff L 1,2 ColKey L 1,2
    _2L_Water
}
*/