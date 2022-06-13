namespace SyneticConverter;
public struct BlendColor
{
    public byte B;
    public byte G;
    public byte R;
    public byte Shadow;

    public override string ToString()
    {
        return $"(R:{R} G:{G} B:{B} Shadow:{Shadow})";
    }
}
