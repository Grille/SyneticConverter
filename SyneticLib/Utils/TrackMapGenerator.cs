using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

using static SyneticLib.Files.QadFile;

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

            DrawLine(nodes[0].Position, nodes[0].Position, 15);
        }

        public void DrawLine(Vector3 pos1, Vector3 pos2, int r)
        {
            var size = new Vector3(Width, Height, Height);
            static float Lerp(float a, float b, float t) => a + (b - a) * t;
            var dist = (int)Vector3.Distance(pos1 * size, pos2 * size) + 1;

            var pl1 = GetPoint(pos1, 0, 0);
            var pl2 = GetPoint(pos2, 0, 0);
            var ps1 = GetPoint(pos1, 0, 5);
            var ps2 = GetPoint(pos2, 0, 5);

            for (int i = 0; i < dist; i++)
            {
                float fac = (float)i / dist;

                int lx = (int)Lerp(pl1.X, pl2.X, fac);
                int ly = (int)Lerp(pl1.Y, pl2.Y, fac);

                int sx = (int)Lerp(ps1.X, ps2.X, fac);
                int sy = (int)Lerp(ps1.Y, ps2.Y, fac);

                FillCircle(lx, ly, r, 1);
                FillCircle(sx, sy, r, 0.5f);
            }
        }

        public (int X, int Y) GetPoint(Vector3 position, float yF, int zOffset)
        {
            float y = position.Y * yF;
            int x = (int)(position.X * Width);
            int z = (int)((1f - position.Z - y) * Height) + zOffset;
            return new(x, z);
        }

        public void FillCircle(Vector3 pos, int rad, float value)
        {
            int x1 = (int)(pos.X * Width);
            int y1 = (int)(pos.Z * Height);

            FillCircle(x1, y1, rad, value);
        }

        public void FillCircle(int cx, int cy, int radius, float value)
        {
            int square = radius;

            for (int iy = -square; iy <= square; iy++)
            {
                for (int ix = -square; ix <= square; ix++)
                {
                    var dist = MathF.Sqrt(MathF.Abs(ix * ix) + MathF.Abs(iy * iy)) - radius * 0.5f;
                    var strength = (1f - Math.Clamp(dist, 0f, 1f)) * value;
                    IncPixel(cx + ix, cy + iy, strength);
                }
            }
        }

        public void IncPixel(int x, int y, float value)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return;
            }

            int idx = x + y * Width;

            byte oldvalue = Data[idx];
            byte newvalue = Math.Clamp((byte)(value * 255), oldvalue, byte.MaxValue);
            Data[idx] = newvalue;
        }
    }
}
