using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace SyneticLib.Compression;

public unsafe static class DdsDecoder
{
    public static void DecodeDxt1ToRgba32(byte* src, byte* dst, int width, int height)
    {
        int blockCountX = (width + 3) / 4;
        int blockCountY = (height + 3) / 4;
        int blockSize = 16;

        for (int y = 0; y < blockCountY; y++)
        {
            for (int x = 0; x < blockCountX; x++)
            {
                byte* blockSrc = src + (y * blockCountX + x) * blockSize;
                DecompressBlockDxt5(blockSrc, dst, x * 4, y * 4, width);
            }
        }
    }

    public static void DecodeDxt5ToRgba32(byte* src, byte* dst, int width, int height)
    {
        int blockCountX = (width + 3) / 4;
        int blockCountY = (height + 3) / 4;
        int blockSize = 16;

        for (int y = 0; y < blockCountY; y++)
        {
            for (int x = 0; x < blockCountX; x++)
            {
                byte* blockSrc = src + (y * blockCountX + x) * blockSize;
                DecompressBlockDxt5(blockSrc, dst, x * 4, y * 4, width);
            }
        }
    }

    private static void DecompressBlockDxt5(byte* block, byte* image, int x, int y, int width)
    {
        // Extract the alpha data
        ulong alphaBits = *((ulong*)block);
        byte alpha0 = (byte)(alphaBits & 0xFF);
        byte alpha1 = (byte)((alphaBits >> 8) & 0xFF);
        alphaBits >>= 16;

        // Extract the color data
        ushort color0 = *((ushort*)(block + 8));
        ushort color1 = *((ushort*)(block + 10));
        uint colorBits = *((uint*)(block + 12));

        // Decompress the alpha channel
        byte[] alphaValues = new byte[8];
        alphaValues[0] = alpha0;
        alphaValues[1] = alpha1;

        if (alpha0 > alpha1)
        {
            for (int i = 2; i < 8; i++)
                alphaValues[i] = (byte)(((8 - i) * alpha0 + (i - 1) * alpha1) / 7);
        }
        else
        {
            for (int i = 2; i < 6; i++)
                alphaValues[i] = (byte)(((6 - i) * alpha0 + (i - 1) * alpha1) / 5);
            alphaValues[6] = 0;
            alphaValues[7] = 255;
        }

        // Decompress the color channel
        Color32[] colors = new Color32[4];
        colors[0] = ColorFrom565(color0);
        colors[1] = ColorFrom565(color1);

        if (color0 > color1)
        {
            colors[2] = new Color32(
                (byte)((2 * colors[0].R + colors[1].R) / 3),
                (byte)((2 * colors[0].G + colors[1].G) / 3),
                (byte)((2 * colors[0].B + colors[1].B) / 3),
                255
            );
            colors[3] = new Color32(
                (byte)((2 * colors[1].R + colors[0].R) / 3),
                (byte)((2 * colors[1].G + colors[0].G) / 3),
                (byte)((2 * colors[1].B + colors[0].B) / 3),
                255
            );
        }
        else
        {
            colors[2] = new Color32(
                (byte)((colors[0].R + colors[1].R) / 2),
                (byte)((colors[0].G + colors[1].G) / 2),
                (byte)((colors[0].B + colors[1].B) / 2),
                255
            );
            colors[3] = new Color32(0, 0, 0, 255);
        }

        // Write the decompressed data to the image buffer
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < 4; i++)
            {
                int pixelIndex = (y + j) * width + (x + i);
                int blockPixelIndex = j * 4 + i;

                byte alphaIndex = (byte)((alphaBits >> (3 * blockPixelIndex)) & 0x07);
                byte colorIndex = (byte)((colorBits >> (2 * blockPixelIndex)) & 0x03);

                Color32 finalColor = colors[colorIndex];
                finalColor.A = alphaValues[alphaIndex];

                *((Color32*)(image + pixelIndex * 4)) = finalColor;
            }
        }
    }

    private static void DecompressBlockDxt1(byte* block, byte* image, int x, int y, int width)
    {
        // Extract the color data
        ushort color0 = *((ushort*)block);
        ushort color1 = *((ushort*)(block + 2));
        uint colorBits = *((uint*)(block + 4));

        // Decompress the color channel
        Color32[] colors = new Color32[4];
        colors[0] = ColorFrom565(color0);
        colors[1] = ColorFrom565(color1);

        if (color0 > color1)
        {
            colors[2] = new Color32(
                (byte)((2 * colors[0].R + colors[1].R) / 3),
                (byte)((2 * colors[0].G + colors[1].G) / 3),
                (byte)((2 * colors[0].B + colors[1].B) / 3),
                255
            );
            colors[3] = new Color32(
                (byte)((2 * colors[1].R + colors[0].R) / 3),
                (byte)((2 * colors[1].G + colors[0].G) / 3),
                (byte)((2 * colors[1].B + colors[0].B) / 3),
                255
            );
        }
        else
        {
            colors[2] = new Color32(
                (byte)((colors[0].R + colors[1].R) / 2),
                (byte)((colors[0].G + colors[1].G) / 2),
                (byte)((colors[0].B + colors[1].B) / 2),
                255
            );
            colors[3] = new Color32(0, 0, 0, 0); // Transparent
        }

        // Write the decompressed data to the image buffer
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < 4; i++)
            {
                int pixelIndex = (y + j) * width + (x + i);
                int blockPixelIndex = j * 4 + i;

                byte colorIndex = (byte)((colorBits >> (2 * blockPixelIndex)) & 0x03);

                *((Color32*)(image + pixelIndex * 4)) = colors[colorIndex];
            }
        }
    }

    private static Color32 ColorFrom565(ushort color)
    {
        byte r = (byte)((color >> 11) & 0x1F);
        byte g = (byte)((color >> 5) & 0x3F);
        byte b = (byte)(color & 0x1F);

        return new Color32(
            (byte)((r << 3) | (r >> 2)),
            (byte)((g << 2) | (g >> 4)),
            (byte)((b << 3) | (b >> 2)),
            255
        );
    }

    private struct Color32
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public Color32(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }
}
