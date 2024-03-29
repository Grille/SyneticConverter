﻿using System.Drawing;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;

namespace SyneticLib.LowLevel;
public struct BgraColor
{
    public byte B;
    public byte G;
    public byte R;
    public byte A;

    public override string ToString()
    {
        return $"(R:{R} G:{G} B:{B} A:{A})";
    }

    public Vector3 ToNormalizedVector3()
    {
        return new Vector3(R / 255f, G / 255f, B / 255f);
    }

    public static BgraColor FromNormalizedVector3(Vector3 vector)
    {
        return FromArgb(0, (byte)(vector.X * 255), (byte)(vector.Y * 255), (byte)(vector.Z * 255));
    }

    public static BgraColor FromArgb(int a, int r, int g, int b) => new BgraColor() { A = (byte)a, R = (byte)r, G = (byte)g, B = (byte)b };

    public static BgraColor FromInt(int a) => Unsafe.As<int, BgraColor>(ref a);
    public static BgraColor FromInt(uint a) => Unsafe.As<uint, BgraColor>(ref a);

    public static implicit operator Color(BgraColor color) => Color.FromArgb(color.A, color.R, color.G, color.B);

    public static implicit operator BgraColor(Color color) => FromArgb(color.A, color.R, color.G, color.B);
}
