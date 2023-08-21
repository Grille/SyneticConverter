using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public static class Textures
{
    public static Texture Checker { get; }

    public static Texture Error { get; }

    static Textures()
    {
        Checker = CreateChecker("Checker", 0xFF969696, 0xFFA9A9A9);
        Error = CreateChecker("Error", 0xFFFF44FF, 0xFF444444);
    }

    static unsafe Texture CreateChecker(string name, uint color1, uint color2)
    {
        byte[] pixels = new byte[2 * 2 * 4];
        fixed (byte* bptr = pixels)
        {
            uint* iptr = (uint*)bptr;
            iptr[0] = iptr[3] = color1;
            iptr[1] = iptr[2] = color2;
        }
        return new Texture(name, TextureFormat.RGBA32, 2, 2, pixels);
    }
}
