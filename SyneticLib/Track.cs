using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class Track
{
    public TrackNode[] Nodes { get; }

    public Track(TrackNode[] nodes)
    {
        Nodes = nodes;
    }

    public void Normalize()
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

        for (int i = 0; i < Nodes.Length; i++)
        {
            ref var position = ref Nodes[i].Position;
            position -= offset;

            position *= scale;
        }
    }

    public Texture CreateTrackMap(int width, int height)
    {
        Normalize();

        var pixels = new byte[width * height];

        for (int i = 0; i < Nodes.Length; i++)
        {
            var node = Nodes[i];
            int x = Math.Clamp((int)(node.Position.X * width), 0, width-1);
            int y = Math.Clamp((int)(node.Position.Z * height), 0, height-1);

            int idx = (x + y * width);
            pixels[idx] = 255;
        }

        var texture = new Texture("track_map", TextureFormat.R8, width, height, pixels);
        return texture;
    }
}
