using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.DataFormats;
using SyneticLib.Utils;

namespace SyneticLib.WinForms.IO;

public static class BitmapConverter
{
    public static Bitmap ConvertToBitmap(Texture texture)
    {
        var bitmap = new Bitmap(texture.Width, texture.Height, PixelFormat.Format32bppArgb);
        var rect = new Rectangle(Point.Empty, bitmap.Size);
        var data = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        ConvertToBitmap(data, texture);
        bitmap.UnlockBits(data);
        return bitmap;
    }

    private static unsafe void ConvertToBitmap(BitmapData data, Texture texture)
    {
        var dst = (byte*)data.Scan0;
        texture.DataToBgra32(dst);
    }

    public static Texture ConvertToTexture(Bitmap bitmap)
    {
        var rect = new Rectangle(Point.Empty, bitmap.Size);
        var data = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        var result = ConvertToTexture(data);
        bitmap.UnlockBits(data);
        return result;
    }

    public static unsafe Texture ConvertToTexture(BitmapData data)
    {
        if (data.PixelFormat != PixelFormat.Format32bppArgb)
        {
            throw new InvalidDataException($"{data.PixelFormat}");
        }
        int size = data.Width * data.Height;
        var bytes = new byte[data.Width * data.Height * 4];
        var ptr = (byte*)data.Scan0;
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = ptr[i];
        }
        return new Texture(TextureFormat.Bgra32, data.Width, data.Height, bytes);
    }
}
