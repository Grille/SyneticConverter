using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public struct Transform
{
    public Transform3 U;
    public Transform3 V;

    public override string ToString()
    {
        return $"{U} {V}";
    }

    public Matrix3 ToMatrix()
    {
        var m = new Matrix3();
        return m;
    }

    public static Transform CreateScale(float x, float z)
    {
        var mat = new Transform();
        mat.U.ScaleX = x;
        mat.V.ScaleZ = z;
        return mat;
    }

    public static Transform CreateScale90Deg(float x, float z)
    {
        var mate = Matrix2x4.CreateScale(x, z);
        return Unsafe.As<Matrix2x4, Transform>(ref mate);
    }

    public static Transform Initial
    {
        get
        {
            var t = new Transform();
            t.U.ScaleX = 1;
            t.U.ScaleY = 1;
            t.V.ScaleY = 1;
            t.V.ScaleZ = 1;
            return t;
        }
    }

    public static Transform Empety
    {
        get
        {
            var t = new Transform();
            t.U.ScaleX = 1;
            t.U.ScaleY = -0;
            t.V.ScaleX = 1;
            t.V.ScaleY = -4.37114E-08f;
            return t;
        }
    }
}
