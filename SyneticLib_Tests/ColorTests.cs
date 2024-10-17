using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib_Tests;

public static class ColorTests
{
    public static void Run()
    {
        Section("Colors");

        Test("Bgra", Bgra);
        Test("Rgb", Rgb);
    }

    static void Bgra()
    {
        var color3 = BgraColor.FromArgb(0, 100, 50, 0);
        var color4 = BgraColor.FromArgb(255, color3.R, color3.G, color3.B);
        var vec3 = color4.ToNormalizedRgbVector3();
        var colorFromVec3 = BgraColor.FromNormalizedRgbVector3(vec3);

        Assert.IsEqual(color3, colorFromVec3);
    }

    static void Rgb()
    {
        var color = RgbColor.FromRgb(100, 50, 0);
        var vec = color.ToNormalizedRgbVector3();
        var colorFromVec = RgbColor.FromNormalizedRgbVector3(vec);
        
        Assert.IsEqual(color, colorFromVec);
    }
}
