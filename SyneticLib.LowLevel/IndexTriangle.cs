using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.LowLevel;
public struct IndexTriangle
{
    public int X, Y, Z;

    public IndexTriangle(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static IndexTriangle operator +(IndexTriangle a, int b)
    {
        return new IndexTriangle(a.X + b, a.Y + b, a.Z + b);
    }
}
