
using SyneticLib.Files.Extra;
using SyneticLib.IO;

namespace SyneticLib_Tests;

internal static class DdsTests
{
    public static void Run()
    {
        Section("DDS");
        Test("Dxt1", Dxt1);
        Test("Dxt5", Dxt5);
        Test("Rgba", Rgba);
    }

    static void Dxt1()
    {
        using var stream = new MemoryStream(Resources.DdsImage_Dxt1);

        var texture = Serializers.Texture.Dds.Deserialize(stream);
    }

    static void Dxt5()
    {
        using var stream = new MemoryStream(Resources.DdsImage_Dxt5);

        var texture = Serializers.Texture.Dds.Deserialize(stream);
    }

    static void Rgba()
    {
        using var stream = new MemoryStream(Resources.DdsImage_Rgba);

        var texture = Serializers.Texture.Dds.Deserialize(stream);
    }
}
