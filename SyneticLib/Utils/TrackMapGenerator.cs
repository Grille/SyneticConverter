using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

namespace SyneticLib.Utils;
public static class TrackMapGenerator
{
    public static Texture CreateTrackMap(Track track, int width, int height, float border = 0.1f)
    {
        var ntrack = track.Clone();

        ntrack.NormalizeXZ();
        ntrack.Scale(1 - border * 2, border);

        var nodes = ntrack.Nodes;

        var pixels = new byte[width * height];

        var ctx = new Ctx()
        {
            Data = pixels,
            Width = width,
            Height = height,
        };
        ctx.DrawNodes(nodes);

        var texture = new Texture(TextureFormat.R8, width, height, pixels);
        return texture;
    }

    struct Ctx
    {
        public byte[] Data;
        public int Width;
        public int Height;

        public void DrawNodes(TrackNode[] nodes)
        {
            for (int i = 0; i < nodes.Length - 1; i++)
            {
                var node0 = nodes[i];
                var node1 = nodes[i + 1];

                DrawLine(node0.Position, node1.Position, 5);
            }

            DrawLine(nodes[0].Position, nodes[0].Position, 10);
        }

        public void DrawLine(Vector3 pos1, Vector3 pos2, int r)
        {
            var size = new Vector3(Width, Height, Height);
            var dist = (int)Vector3.Distance(pos1 * size, pos2 * size) + 1;

            var pl1 = GetPoint(pos1, 0, 0);
            var pl2 = GetPoint(pos2, 0, 0);
            var ps1 = GetPoint(pos1, 0, 2);
            var ps2 = GetPoint(pos2, 0, 2);

            for (int i = 0; i < dist; i++)
            {
                float fac = (float)i / dist;

                var pl = Vector2.Lerp(pl1, pl2, fac);
                var ps = Vector2.Lerp(ps1, ps2, fac);

                FillCircle(pl, r, 1);
                FillCircle(ps, r, 0.5f);
            }
        }

        public Vector2 GetPoint(Vector3 position, float yF, int zOffset)
        {
            float y = position.Y * yF;
            float x = position.X * Width;
            float z = ((1f - position.Z) * Height) + zOffset;
            return new(x, z);
        }

        public void FillCircle(Vector3 pos, float rad, float value)
        {
            FillCircle(new Vector2(pos.X * Width, pos.Z * Height), rad, value);
        }

        public void FillCircle(Vector2 center, float radius, float value)
        {
            int square = (int)radius;

            int icx = (int)center.X;
            int icy = (int)center.Y;

            for (float iy = -square; iy <= square; iy++)
            {
                for (float ix = -square; ix <= square; ix++)
                {
                    var offset = new Vector2(ix, iy);
                    var position = center + offset;

                    var rounded = (Vector2i)position;

                    var dist = Vector2.Distance(center, rounded);
                    float clamped = Math.Clamp(dist - radius + 2.5f, 0f, 2f) * 0.5f;
                    var strength = (1f - clamped) * value;
                    IncPixel(rounded, strength);
                }
            }
        }

        public void IncPixel(Vector2i pos, float value)
        {
            if (pos.X < 0 || pos.X >= Width || pos.Y < 0 || pos.Y >= Height)
            {
                return;
            }

            int idx = pos.X + pos.Y * Width;

            byte oldvalue = Data[idx];
            byte newvalue = Math.Clamp((byte)(value * 255), oldvalue, byte.MaxValue);
            Data[idx] = newvalue;
        }
    }
}
