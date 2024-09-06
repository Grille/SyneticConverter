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
    public static Texture CreateTrackMap(Track track, int width, int height)
    {
        track.NormalizeXZ();
        track.Scale(0.8f, 0.1f);

        var nodes = track.Nodes;

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
            var size = new Vector3(Width, 1, Height);
            static float Lerp(float a, float b, float t) => a + (b - a) * t;
            var dist = (int)Vector3.Distance(pos1 * size, pos2 * size) + 1;

            int x1 = (int)(pos1.X * Width);
            int y1 = (int)((1f - pos1.Z) * Height);
            int x2 = (int)(pos2.X * Width);
            int y2 = (int)((1f - pos2.Z) * Height);

            //FillCircle(x1, y1, 5, 0.3f);
            //FillCircle(x2, y2, 5, 0.3f);

            for (int i = 0; i < dist; i++)
            {
                float fac = (float)i / dist;

                int x = (int)Lerp(x1, x2, fac);
                int y = (int)Lerp(y1, y2, fac);

                FillCircle(x, y, r, 1);
                FillCircle(x, y + 5, r, 0.5f);
            }

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
