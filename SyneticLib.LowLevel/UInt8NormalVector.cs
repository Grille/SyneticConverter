using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

namespace SyneticLib;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
public struct UInt8NormalVector
{
    public byte Z, Y, X, W;

    public UInt8NormalVector(byte x, byte y, byte z)
    {
        X = x;
        Y = y;
        Z = z;
        W = 0;
    }

    public static byte Encode(float value)
    {
        return (byte)((value + 1f) / 2f);
    }

    public static float Decode(byte value)
    {
        return value / 128f - 1f;
    }

    public static Vector3 Decode(UInt8NormalVector value)
    {
        return new Vector3(Decode(value.X), Decode(value.Y), Decode(value.Z));
    }

    public static UInt8NormalVector Encode(Vector3 value)
    {
        return new UInt8NormalVector(Encode(value.X), Encode(value.Y), Encode(value.Z));
    }

    public static implicit operator Vector3(UInt8NormalVector value) => Decode(value);

    public static explicit operator UInt8NormalVector(Vector3 value) => Encode(value);
}
