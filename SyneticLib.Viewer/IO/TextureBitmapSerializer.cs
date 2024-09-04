using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.Files.Extra;
using SyneticLib.IO;

namespace SyneticLib.WinForms.IO;
public class TextureBitmapSerializer : IFileSerializer<Texture>
{
    public ImageFormat Format { get; }

    public TextureBitmapSerializer(ImageFormat format)
    {
        Format = format;
    }

    public void Save(string filePath, Texture value)
    {
        using var stream = File.Create(filePath);
        Serialize(stream, value);
    }

    public void Serialize(Stream stream, Texture texture)
    {
        using var bitmap = BitmapConverter.ConvertToBitmap(texture);
        bitmap.Save(stream, Format);
    }



    public Texture Load(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        return Deserialize(stream);
    }

    public Texture Deserialize(Stream stream)
    {
        using var bitmap = new Bitmap(stream);

        if (bitmap.RawFormat.Guid != Format.Guid)
        {
            throw new InvalidDataException($"{bitmap.RawFormat}");
        }

        return BitmapConverter.ConvertToTexture(bitmap);
    }
}
