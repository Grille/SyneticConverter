using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;

public unsafe class TextureLevel
{
    public int Width { get; }

    public int Height { get; }

    public int Length => Width * Height;

    public int Stride => Data.Length / Length;

    public byte[] Data { get; }

    public TextureLevel(int width, int height, byte[] data, bool copyData = false)
    {
        Width = width;
        Height = height;
        Data = data;
    }

    public void FlipY()
    {
        int width = Width;
        int height = Height;
        var data = Data;
        int stride = Stride;

        int halfHeight = Height / 2;
        int byteWidth = width * stride;

        for (int iy = 0; iy < halfHeight; iy++)
        {
            int y0 = iy;
            int y1 = height - iy - 1;

            int idx0 = y0 * byteWidth;
            int idx1 = y1 * byteWidth;

            for (int ix = 0; ix < byteWidth; ix++)
            {
                var temp = data[idx1 + ix];
                data[idx1 + ix] = data[idx0 + ix];
                data[idx0 + ix] = temp;
            }
        }
    }

    public TextureLevel Clone()
    {
        return new TextureLevel(Width, Height, (byte[])Data.Clone());
    }
}
