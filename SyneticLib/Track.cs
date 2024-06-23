using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

namespace SyneticLib;
public class Track
{
    public TrackNode[] Nodes { get; }

    public Track(TrackNode[] nodes)
    {
        Nodes = nodes;
    }

    public void NormalizeXZ()
    {
        if (Nodes.Length == 0)
        {
            return;
        }

        var boundings = new BoundingBox(Nodes[0].Position);

        for (int i = 0; i < Nodes.Length; i++)
        {
            boundings.Extend(Nodes[i].Position);
        }

        var offset = boundings.Start;
        var size = boundings.Size;

        float scale = 1 / MathF.Max(size.X, size.Z);
        var centerOffset = (Vector3.One - (size * scale)) * 0.5f;

        for (int i = 0; i < Nodes.Length; i++)
        {
            ref var position = ref Nodes[i].Position;
            position -= offset;
            position *= scale;
            position += centerOffset;
        }
    }

    public void Scale(float scale, float offset = 0)
    {
        Scale(new Vector3(scale), new Vector3(offset));
    }

    public void Scale(Vector3 scale, Vector3 offset)
    {
        for (int i = 0; i < Nodes.Length; i++)
        {
            ref var position = ref Nodes[i].Position;
            position *= scale;
            position += offset;
        }
    }

    public Texture CreateTrackMap(int width, int height)
    {
        NormalizeXZ();
        Scale(0.8f, 0.1f);

        var pixels = new byte[width * height];

        void IncPixel(int x, int y, byte value)
        {
            int idx = (x + y * width);
            pixels[idx] = Math.Max(pixels[idx], value);
        }

        for (int i = 0; i < Nodes.Length; i++)
        {
            var node = Nodes[i];
            int x = (int)(node.Position.X * width);
            int y = (int)(node.Position.Z * height);

            int square = 5;

            for (int iy = -square; iy <= square; iy++)
            {
                for (int ix = -square; ix <= square; ix++)
                {
                    var dist = MathF.Sqrt(MathF.Abs(ix * ix) + MathF.Abs(iy * iy)) * 0.5f - 1.1f;
                    var factor = (1f - Math.Clamp(dist, 0f, 1f));
                    var color = (byte)(255 * factor);
                    IncPixel(x + ix, y + iy, color);
                }
            }
        }

        var texture = new Texture("track_map", TextureFormat.R8, width, height, pixels);
        return texture;
    }
}
