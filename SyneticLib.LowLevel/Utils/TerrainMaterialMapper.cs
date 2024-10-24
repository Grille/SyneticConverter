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
using System.Runtime.CompilerServices;

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

    public static void ConvertWR1ToWR2(ref this QadFile.AbstractMaterialType mat)
    {
        (var zOffset, var type) = Decode<WR1Type>(mat.Layer0.Mode);
        WR2Type result;

        switch (type)
        {
            case WR1Type.Terrain:
            {
                result = WR2Type.Terrain;
                break;
            }
            case WR1Type.Road:
            {
                result = WR2Type.Road;
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
            case WR1Type.Windows:
            {
                result = WR2Type.Windows;
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
            case WR1Type.Water:
            {
                result = WR2Type.Water;
                break;
            }
            case WR1Type.Colorkey:
            {
                result = WR2Type.Colorkey;
                break;
            }
            case WR1Type.Alpha:
            {
                result = WR2Type.Alpha;
                break;
            }
            default:
            {
                throw new NotImplementedException(type.ToString());
            }
        }

        mat.Layer0.Mode = Encode(zOffset, result);
    }

    public static void ConvertC11ToWR2(ref QadFile.AbstractMaterialType mat, TextureTransform[] matrices)
    {
        var dmat0 = matrices[mat.Layer0.Tex0Id];
        var dmat1 = matrices[mat.Layer0.Tex1Id];
        var dmat2 = matrices[mat.Layer0.Tex2Id];

        mat.Matrix0 = MatrixUV;
        mat.Matrix1 = MatrixUV;
        mat.Matrix2 = MatrixUV;

        (var zOffset, var type) = Decode<C11Type>(mat.Layer1.Mode);
        WR2Type result;

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
            {
                result = WR2Type.L2ReflOvl;
                break;
            }
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

        mat.Layer0.Mode = Encode(zOffset, result);
    }

    public unsafe static (int ZOffset, T Type) Decode<T>(QadFile.MMaterialMode mode) where T : unmanaged
    {
        AssertIsInteger<T>();
        int zOffset = mode & 15;
        int type = mode >> 4;
        return (zOffset, Unsafe.As<int, T>(ref type));
    }

    public unsafe static QadFile.MMaterialMode Encode<T>(int zOffset, T type) where T : unmanaged
    {
        AssertIsInteger<T>();
        return (QadFile.MMaterialMode)((Unsafe.As<T, int>(ref type) << 4) | zOffset);
    }

    static void AssertIsInteger<T>() where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != sizeof(int)) throw new ArgumentException();
    }
}
