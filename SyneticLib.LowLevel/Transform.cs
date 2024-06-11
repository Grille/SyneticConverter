using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public struct Transform
{
    public Transform3 X;
    public Transform2 Y;
    public Transform3 Z;

    public override string ToString()
    {
        return $"{X.Rotate} {X.Scale} {X.Move} {Y.Rotate} {Y.Scale} {Z.Rotate} {Z.Scale} {Z.Move}";
    }

    public Matrix3 ToMatrix()
    {
        var m = new Matrix3();
        return m;
    }

    public static Transform Empety
    {
        get
        {
            var t = new Transform();
            t.X.Rotate = 1;
            t.X.Scale = -0;
            t.Z.Rotate = 1;
            t.Z.Scale = -4.37114E-08f;
            return t;
        }
    }
}
