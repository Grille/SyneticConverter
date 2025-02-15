using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;

namespace SyneticLib;
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

    public string ToHexString()
    {
        return ((int)this).ToString("", CultureInfo.InvariantCulture);
    }

    static byte Normalize(float v) => (byte)(v * 255);

    public BgraColor ParseHex(string hex)
    {
        int nr = int.Parse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        return nr;
    }

    public Vector3 ToNormalizedRgbVector3()
    {
        return new Vector3(R / 255f, G / 255f, B / 255f);
    }

    public static BgraColor FromNormalizedRgbVector3(Vector3 vector)
    {
        return FromArgb(255, Normalize(vector.X), Normalize(vector.Y), Normalize(vector.Z));
    }

    public Vector4 ToNormalizedRgbaVector4()
    {
        return new Vector4(R / 255f, G / 255f, B / 255f, A / 255);
    }

    public static BgraColor FromNormalizedRgbaVector4(Vector4 vector)
    {
        return FromArgb(Normalize(vector.W), Normalize(vector.X), Normalize(vector.Y), Normalize(vector.Z));
    }

    public static BgraColor FromArgb(int a, int r, int g, int b) => new BgraColor() { A = (byte)a, R = (byte)r, G = (byte)g, B = (byte)b };

    public static BgraColor FromArgb(int a) => Color.FromArgb(a);

    public static BgraColor FromBgra(int a) => Unsafe.As<int, BgraColor>(ref a);

    public static BgraColor FromBgra(uint a) => Unsafe.As<uint, BgraColor>(ref a);

    public static implicit operator int(BgraColor color) => Unsafe.As<BgraColor, int>(ref color);

    public static implicit operator BgraColor(int color) => FromBgra(color);

    public static implicit operator Color(BgraColor color) => Color.FromArgb(color.A, color.R, color.G, color.B);

    public static implicit operator BgraColor(Color color) => FromArgb(color.A, color.R, color.G, color.B);
}
