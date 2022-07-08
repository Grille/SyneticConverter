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

    public Vector3 GetBlendAsVec3()
    {
        return new Vector3(B0 / 255f, B1 / 255f, B2 / 255f);
    }

    public float GetShadowAsFloat()
    {
        return Shadow / 255f;
    }

}
