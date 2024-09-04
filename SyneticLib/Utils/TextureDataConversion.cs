using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Compression;

using static SyneticLib.Files.LvlFile;
using static SyneticLib.IO.Serializers;

namespace SyneticLib.Utils;
public static unsafe class TextureDataConversion
{
    public static void DataToRgba32(byte* src, byte* dst, TextureFormat format, int width, int height)
    {
        int size = width * height;

        switch (format)
        {
            case TextureFormat.R8:
            {
                for (int i = 0; i < size; i++)
                {
                    dst[i * 4 + 0] = src[i]; // R
                    dst[i * 4 + 1] = src[i]; // G
                    dst[i * 4 + 2] = src[i]; // B
                    dst[i * 4 + 3] = src[i]; // A
                }
                break;
            }
            case TextureFormat.Rgb24:
            {
                for (int i = 0; i < size; i++)
                {
                    dst[i * 4 + 0] = src[i * 3 + 0];
                    dst[i * 4 + 1] = src[i * 3 + 1];
                    dst[i * 4 + 2] = src[i * 3 + 2];
                    dst[i * 4 + 3] = 255;
                }
                return;
            }
            case TextureFormat.Rgba32:
            {
                int sizex4 = size * 4;
                new Span<byte>(src, sizex4).CopyTo(new Span<byte>(dst, sizex4));
                return;
            }
            case TextureFormat.Bgra32:
            {
                for (int i = 0; i < size; i++)
                {
                    dst[i * 4 + 0] = src[i * 4 + 2]; // B = R
                    dst[i * 4 + 1] = src[i * 4 + 1]; // G = G
                    dst[i * 4 + 2] = src[i * 4 + 0]; // R = B
                    dst[i * 4 + 3] = src[i * 4 + 3]; // A = A
                }
                return;
            }
            case TextureFormat.Rgb24Dxt1:
            {
                DdsDecoder.DecodeDxt1ToRgba32(src, dst, width, height);
                return;
            }
            case TextureFormat.Rgba32Dxt5:
            {
                DdsDecoder.DecodeDxt5ToRgba32(src, dst, width, height);
                return;
            }
            default:
            {
                throw new InvalidOperationException();
            }
        }
    }

    public static void DataToBgra32(byte* src, byte* dst, TextureFormat format, int width, int height)
    {
        DataToRgba32(src, dst, format, width, height);
        int size = width * height;
        for (int i = 0; i < size; i++)
        {
            byte b = dst[i * 4 + 2];
            dst[i * 4 + 2] = dst[i * 4 + 0];
            dst[i * 4 + 0] = b;
        }
    }

    public static byte[] DataToRgba32(this Texture texture, int mipmapLevelIdx = 0)
    {
        var data = CreateDstBuffer(texture, 4, mipmapLevelIdx);
        fixed (byte* dst = data)
        {
            DataToRgba32(texture, dst, mipmapLevelIdx);
        }
        return data;
    }

    public static void DataToRgba32(this Texture texture, byte* dst, int mipmapLevelIdx = 0)
    {
        var level = texture.Levels[mipmapLevelIdx];
        fixed (byte* src = level.Data)
        {
            DataToRgba32(src, dst, texture.Format, level.Width, level.Height);
        }
    }

    public static byte[] DataToBgra32(this Texture texture, int mipmapLevelIdx = 0)
    {
        var data = CreateDstBuffer(texture, 4, mipmapLevelIdx);
        fixed (byte* dst = data)
        {
            DataToBgra32(texture, dst, mipmapLevelIdx);
        }
        return data;
    }

    public static void DataToBgra32(this Texture texture, byte* dst, int mipmapLevelIdx = 0)
    {
        var level = texture.Levels[mipmapLevelIdx];
        fixed (byte* src = level.Data)
        {
            DataToBgra32(src, dst, texture.Format, level.Width, level.Height);
        }
    }

    private static byte[] CreateDstBuffer(Texture texture, int stride, int mipmapLevelIdx)
    {
        var level = texture.Levels[mipmapLevelIdx];
        return new byte[level.Width * level.Height * stride];
    }
}
