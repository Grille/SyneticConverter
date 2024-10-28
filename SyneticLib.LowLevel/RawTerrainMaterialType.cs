using SyneticLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;

public enum TerrainMaterialTypeWR1 : int
{
    Terrain,
    Road,
    L1Refl,
    L1Spec,
    Windows,
    L2SpecFaded,
    L2SpecOverlay,
    L2SpecOverlayT,
    Unknown08,
    Unknown09,
    Unknown10,
    Unknown11,
    Unknown12,
    Water,
    Colorkey,
    Alpha,
}

public enum TerrainMaterialTypeWR2 : int
{
    L3Terrain,
    L2Road,
    L1Refl,
    L2Refl1,
    L2Refl2,
    L2ReflOvlT,
    L2ReflOvl,
    L1Windows,
    L1Spec,
    L2SpecFaded,
    L2SpecOverlayT,
    L2SpecOverlay,
    Unknown12,
    L1ColorKey,
    L1Water,
    L1Alpha,
}

public enum TerrainMaterialTypeC11 : int
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
    L2DiffDiff12ColorKey1,
    L2ReflDiff12ColorKey12,
    L2Water
}
