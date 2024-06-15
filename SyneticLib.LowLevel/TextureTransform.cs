using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public struct TextureTransform
{
    public struct Transform4
    {
        public float ScaleX;
        public float ScaleY;
        public float ScaleZ;
        public float Offset;

        public static implicit operator Vector4(Transform4 v) => Unsafe.As<Transform4, Vector4>(ref v);
        public static implicit operator Transform4(Vector4 v) => Unsafe.As<Vector4, Transform4>(ref v);

        public override string ToString() => ((Vector4)this).ToString();
    }

    public Transform4 U;
    public Transform4 V;

    public readonly static TextureTransform Initial;
    public readonly static TextureTransform Empety;

    static TextureTransform()
    {
        var t = new TextureTransform();
        t.U.ScaleX = 1;
        t.U.ScaleY = -0;
        t.V.ScaleX = 1;
        t.V.ScaleY = -4.37114E-08f;
        Empety =  t;

        Initial = CreateScale(1, 1);
    }

    public override string ToString()
    {
        return $"{U} {V}";
    }

    public Matrix3 ToMatrix()
    {
        var m = new Matrix3();
        return m;
    }

    public static TextureTransform CreateScale(float x, float z)
    {
        var mat = new TextureTransform();
        mat.U.ScaleX = x;
        mat.V.ScaleZ = z;
        return mat;
    }

    public static TextureTransform CreateScale90Deg(float x, float z)
    {
        var mate = Matrix2x4.CreateScale(x, z);
        return Unsafe.As<Matrix2x4, TextureTransform>(ref mate);
    }
}
