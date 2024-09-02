using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SyneticLib.Files.QadFile;

namespace SyneticLib.Utils;
public static class TrackMapGenerator
{
    public static Texture CreateTrackMap(Track track, int width, int height)
    {
        track.NormalizeXZ();
        track.Scale(0.8f, 0.1f);

        var nodes = track.Nodes;

        var pixels = new byte[width * height];

        Gen(nodes, pixels, width, height);

        var texture = new Texture(TextureFormat.R8, width, height, pixels);
        return texture;
    }

    private static void Gen(TrackNode[] nodes, byte[] pixels, int width, int height)
    {
        void IncPixel(int x, int y, byte value)
        {
            int idx = (x + y * width);
            pixels[idx] = Math.Max(pixels[idx], value);
        }

        for (int i = 0; i < nodes.Length; i++)
        {
            var node = nodes[i];
            int x = (int)(node.Position.X * width);
            int y = (int)(node.Position.Z * height);

            int square = 5;

            for (int iy = -square; iy <= square; iy++)
            {
                for (int ix = -square; ix <= square; ix++)
                {
                    var dist = MathF.Sqrt(MathF.Abs(ix * ix) + MathF.Abs(iy * iy)) * 0.5f - 1.1f;
                    var factor = (1f - Math.Clamp(dist, 0f, 1f));
                    var strength = (byte)(255 * factor);
                    IncPixel(x + ix, y + iy, strength);
                }
            }
        }
    }

    private static void FillCircle()
    {

    }
}
