using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

namespace SyneticLib;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RgbColor
{
    public byte B;
    public byte G;
    public byte R;

    public Vector3 ToNormalizedRgbVector3()
    {
        float ToSingle(byte v) => v / 255f;
        return new Vector3(ToSingle(R), ToSingle(G), ToSingle(B));
    }

    public static RgbColor FromNormalizedRgbVector3(Vector3 vector)
    {
        byte ToByte(float v) => (byte)(v * 255f);
        return FromRgb(ToByte(vector.X), ToByte(vector.Y), ToByte(vector.Z));
    }

    public static RgbColor FromRgb(byte r, byte g, byte b) => new RgbColor() { R = r, G = g, B = b };
}
