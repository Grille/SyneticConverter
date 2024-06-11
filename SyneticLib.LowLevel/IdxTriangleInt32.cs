using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;

public struct IdxTriangleInt32
{
    public int X, Y, Z;

    public IdxTriangleInt32(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static IdxTriangleInt32 operator +(IdxTriangleInt32 a, int b)
    {
        return new IdxTriangleInt32(a.X + b, a.Y + b, a.Z + b);
    }
}

public struct IdxTriangleUInt16
{
    public ushort X, Y, Z;

    public IdxTriangleUInt16(ushort x, ushort y, ushort z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static explicit operator IdxTriangleUInt16(IdxTriangleInt32 a) => new IdxTriangleUInt16((ushort)a.X, (ushort)a.Y, (ushort)a.Z);

    public static implicit operator IdxTriangleInt32(IdxTriangleUInt16 a) => new IdxTriangleInt32(a.X, a.Y, a.Z);
}

