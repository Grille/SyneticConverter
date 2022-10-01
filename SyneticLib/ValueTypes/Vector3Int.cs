using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public struct Vector3Int
{
    public int X, Y, Z;

    public Vector3Int(int x,int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static Vector3Int operator +(Vector3Int a, int b)
    {
        return new Vector3Int(a.X + b, a.Y + b, a.Z + b);
    }
}
