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

    public Track Clone()
    {
        var nodes = new TrackNode[Nodes.Length];
        for (int i = 0; i< nodes.Length; i++)
        {
            nodes[i] = Nodes[i];
        }
        return new Track(nodes);
    }

    public float GetAbsDistance()
    {
        float distance = 0;

        int count = Nodes.Length-1;

        for (int i = 0; i < count; i++)
        {
            var thisPos = Nodes[i+0].Position;
            var nextPos = Nodes[i+1].Position;

            distance += Vector3.Distance(thisPos, nextPos);
        }

        return distance / count;
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
}
