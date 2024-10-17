using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;

using C11Type = SyneticLib.TerrainMaterialTypeC11;
using WR2Type = SyneticLib.TerrainMaterialTypeWR2;

namespace SyneticLib.Utils;

public static class TerrainMaterialMapper
{
    static readonly TextureTransform MatrixUV;
    static readonly TextureTransform Matrix64;
    static readonly TextureTransform Matrix128;
    static readonly TextureTransform Matrix256;
    static readonly TextureTransform Matrix512;

    static TerrainMaterialMapper()
    {
        MatrixUV = TextureTransform.CreateScale90Deg(1, 1);
        Matrix64 = TextureTransform.CreateScale(1f / 64f, 1f / 64f);
        Matrix128 = TextureTransform.CreateScale(1f / 128f, 1f / 128f);
        Matrix256 = TextureTransform.CreateScale(1f / 256f, 1f / 256f);
        Matrix512 = TextureTransform.CreateScale(1f / 512f, 1f / 512f);
    }

    public static void ConvertC11ToWR2(ref QadFile.AbstractMaterialType mat, TextureTransform[] matrices)
    {
        var dmat0 = matrices[mat.Layer0.Tex0Id];
        var dmat1 = matrices[mat.Layer0.Tex1Id];
        var dmat2 = matrices[mat.Layer0.Tex2Id];

        mat.Matrix0 = MatrixUV;
        mat.Matrix1 = MatrixUV;
        mat.Matrix2 = MatrixUV;

        int mode = mat.Layer1.Mode;
        int zOffset = mode & 15;
        var type = (C11Type)(mode >> 4);
        var result = WR2Type.Terrain;

        switch (type)
        {
            case C11Type.L2Water:
            {
                result = WR2Type.Water;
                mat.Matrix0 = dmat0;
                break;
            }
            case C11Type.L2ReflDiff12ColKey12:
            case C11Type.L2DiffDiff12ColKey1:
            {
                result = WR2Type.Colorkey;
                break;
            }
            case C11Type.L3SpecSpecDiff:
            {
                result = WR2Type.L2SpecFaded;
                break;
            }
            case C11Type.L3ReflReflDiff:
            {
                result = WR2Type.L2Refl2;
                break;
            }
            case C11Type.L3ReflDiffDiff:
            {
                result = WR2Type.Road;
                mat.Matrix2 = dmat2;
                break;
            }
            case C11Type.L2ReflDiffWin12:
            {
                result = WR2Type.Windows;
                break;
            }
            case C11Type.L2ReflDiff13:
            case C11Type.L2ReflDiff12:
            case C11Type.L1Spec:
            {
                result = WR2Type.L1Spec;
                break;
            }
            case C11Type.L1Refl:
            {
                result = WR2Type.L1Refl;
                break;
            }
            case C11Type.L2DiffDiff12:
            case C11Type.L1Diff:
            {
                result = WR2Type.Road;
                mat.Matrix2 = dmat2;
                break;
            }
            case C11Type.L3DiffDiffDiff:
            case C11Type.L4Diff1234:
            {
                result = WR2Type.Terrain;
                mat.Matrix0 = dmat0;
                mat.Matrix1 = dmat1;
                mat.Matrix2 = dmat2;
                break;
            }
            default:
            {
                throw new NotImplementedException(type.ToString());
            }

        }

        mat.Layer0.Mode = (QadFile.MMaterialMode)(((int)result << 4) | zOffset);
    }
}
