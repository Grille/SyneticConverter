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

}