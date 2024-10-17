using SyneticLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;

public enum OldTerrainMaterialTypeWR2 : ushort
{
    Terrain = 0,
    Road = 16,//1
    UV = 32,//2
    Reflective = 112,//7
    Road0 = 128,//8
    Road1 = 144,
    Road2 = 160,
    Road3 = 176,
    AlphaClip = 208,
    Water = 224,
    AlphaBlend = 240,
}

/*
 * public enum TerrainMaterialTypeWR2 : ushort
{
    Terrain = 0,
    Road = 16,//1
    UV = 32,//2
    Reflective = 112,//7
    Road0 = 128,//8
    Road1 = 144,
    Road2 = 160,
    Road3 = 176,
    AlphaClip = 208,
    Water = 224,
    AlphaBlend = 240,
}
*/

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

public enum OldTerrainMaterialTypeC11 : ushort
{
    Terrain = 0,
    UVTerrain = 16,//L1Diff
    UV = 32,//L1Refl
    Road0 = 240,//L3ReflDiffDiff 15
    Reflective = 192,//L2ReflDiffWin12 12
    AlphaClip = 288,//L2DiffDiff12ColKey1
    Water = 320,//L2Water
    AlphaBlend = 240,//L3ReflReflDiff
}

public enum TerrainMaterialTypeWR2 : ushort
{
    Terrain,
    Road,
    L1Refl,
    L2Refl1,
    L2Refl2,
    L2ReflOvlT,
    L2ReflOvl,
    Windows,
    L1Spec,
    L2SpecFaded,
    L2SpecOverlayT,
    L2SpecOverlay,
    Unused0,
    Colorkey,
    Water,
    Alpha,
}

public enum TerrainMaterialTypeC11 : ushort
{
    L4Diff1234,
    L1Diff,
    L1Refl,
    L1metal,
    L1Spec,
    L2DiffDiff12,
    L2DiffDiff13,
    L2ReflDiff12,
    L2ReflDiff13,
    L2MetalDiff12,
    L2SpecSpec12,
    L2SpecDiff13,
    L2ReflDiffWin12,
    L3DiffDiffDiff,
    L3ReflDiffDiff,
    L3ReflReflDiff,
    L3SpecSpecDiff,
    L3SpecDiffDiff,
    L2DiffDiff12ColKey1,
    L2ReflDiff12ColKey12,
    L2Water
}
