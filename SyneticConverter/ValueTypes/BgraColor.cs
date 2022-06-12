namespace SyneticConverter;
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
}
