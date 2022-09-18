using System.Numerics;

namespace SyneticLib;
public struct BlendColor
{
    public byte B0;
    public byte B1;
    public byte B2;
    public byte Shadow;

    public override string ToString()
    {
        return $"(R:{B2} G:{B1} B:{B0} Shadow:{Shadow})";
    }

    public Vector3 Vec3Blend
    {
        get => new Vector3(B0 / 255f, B1 / 255f, B2 / 255f);
        set
        {
            B0 = (byte)(value.X * 255);
            B1 = (byte)(value.Y * 255);
            B2 = (byte)(value.Z * 255);
        }
    }

    public float FloatShadow
    {
        get => Shadow / 255f;
        set => Shadow = (byte)(value * 255);
    }
}
