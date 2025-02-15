using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;

using WR1Type = SyneticLib.TerrainMaterialTypeWR1;
using WR2Type = SyneticLib.TerrainMaterialTypeWR2;
using C11Type = SyneticLib.TerrainMaterialTypeC11;

namespace SyneticLib.Utils;

public static class TerrainMaterialMapper
{
    public static readonly TextureTransform MatrixUV;
    public static readonly TextureTransform Matrix64;
    public static readonly TextureTransform Matrix128;
    public static readonly TextureTransform Matrix256;
    public static readonly TextureTransform Matrix512;

    static TerrainMaterialMapper()
    {
        MatrixUV = TextureTransform.CreateScale90Deg(1, 1);
        Matrix64 = TextureTransform.CreateScale(1f / 64f, 1f / 64f);
        Matrix128 = TextureTransform.CreateScale(1f / 128f, 1f / 128f);
        Matrix256 = TextureTransform.CreateScale(1f / 256f, 1f / 256f);
        Matrix512 = TextureTransform.CreateScale(1f / 512f, 1f / 512f);
    }

    public static void ConvertWR1ToWR2(ref this QadFile.AbstractMaterialType mat)
    {
        (var zOffset, var type) = mat.Layer0.Mode.Decode<WR1Type>();
        WR2Type result;

        switch (type)
        {
            case WR1Type.L3Terrain:
            {
                result = WR2Type.L3Terrain;
                break;
            }
            case WR1Type.L2Road:
            {
                result = WR2Type.L2Road;
                mat.Matrix1 = MatrixUV;
                break;
            }
            case WR1Type.L1Refl:
            {
                result = WR2Type.L1Refl;
                break;
            }
            case WR1Type.L1Spec:
            {
                result = WR2Type.L1Spec;
                break;
            }
            case WR1Type.L1Windows:
            {
                result = WR2Type.L1Windows;
                break;
            }
            case WR1Type.L2SpecFaded:
            {
                result = WR2Type.L2SpecFaded;
                mat.Matrix0 = MatrixUV;
                mat.Matrix1 = MatrixUV;
                mat.Matrix2 = MatrixUV;
                break;
            }
            case WR1Type.L2SpecOverlay:
            {
                result = WR2Type.L2SpecOverlay;
                mat.Matrix0 = MatrixUV;
                mat.Matrix1 = MatrixUV;
                mat.Matrix2 = MatrixUV;
                break;
            }
            case WR1Type.L2SpecOverlayT:
            {
                result = WR2Type.L2SpecOverlayT;
                break;
            }
            case WR1Type.L1Water:
            {
                result = WR2Type.L1Water;
                break;
            }
            case WR1Type.L1Colorkey:
            {
                result = WR2Type.L1ColorKey;
                break;
            }
            case WR1Type.L1Alpha:
            {
                result = WR2Type.L1Alpha;
                break;
            }
            default:
            {
                throw new NotImplementedException(type.ToString());
            }
        }

        mat.Layer0.Mode.Encode(zOffset, result);
    }

    public static void ConvertC11ToWR2(ref QadFile.AbstractMaterialType mat, TextureTransform[] matrices)
    {
        var dmat0 = matrices[mat.Layer0.Tex0Id];
        var dmat1 = matrices[mat.Layer0.Tex1Id];
        var dmat2 = matrices[mat.Layer0.Tex2Id];

        mat.Matrix0 = MatrixUV;
        mat.Matrix1 = MatrixUV;
        mat.Matrix2 = MatrixUV;

        (var zOffset, var type) = mat.Layer1.Mode.Decode<C11Type>();
        WR2Type result;

        switch (type)
        {
            case C11Type.L2Water:
            {
                result = WR2Type.L1Water;
                mat.Matrix0 = dmat0;
                break;
            }
            case C11Type.L1metal:
            {
                result = WR2Type.L1Refl;
                mat.Matrix0 = dmat0;
                break;
            }
            case C11Type.L2ReflDiff12ColorKey12:
            case C11Type.L2DiffDiff12ColorKey1:
            {
                result = WR2Type.L1ColorKey;
                break;
            }
            case C11Type.L3SpecSpecDiff:
            {
                result = WR2Type.L2SpecFaded;
                break;
            }
            case C11Type.L3ReflDiffDiff:
            {
                result = WR2Type.L2Road;
                mat.Matrix2 = dmat2;
                break;
            }
            case C11Type.L2ReflDiffWin12:
            {
                result = WR2Type.L1Windows;
                break;
            }
            case C11Type.L2ReflDiff13:
            case C11Type.L2ReflDiff12:
            case C11Type.L1Spec:
            {
                result = WR2Type.L1Spec;
                break;
            }
            case C11Type.L1Diff:
            case C11Type.L1Refl:
            {
                result = WR2Type.L1Refl;
                break;
            }
            case C11Type.L3DiffDiffDiff:
            case C11Type.L2DiffDiff12:
            {
                result = WR2Type.L2Road;
                mat.Matrix2 = dmat2;
                break;
            }
            case C11Type.L3ReflReflDiff:
            case C11Type.L4Diff1234:
            {
                result = WR2Type.L3Terrain;
                mat.Matrix0 = dmat0;
                mat.Matrix1 = dmat1;
                mat.Matrix2 = dmat2;
                break;
            }
            case C11Type.L2MetalDiff12:
            case C11Type.L2DiffDiff13:
            {
                result = WR2Type.L1Refl;
                break;
            }
            default:
            {
                throw new NotImplementedException(type.ToString());
            }
        }

        mat.Layer0.Mode.Encode(zOffset, result);
    }
}
