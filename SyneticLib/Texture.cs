using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class Texture : Ressource
{
    public TextureFormat Format { get; }

    public TextureLevel[] Levels { get; }

    public int Width => Levels[0].Width;

    public int Height => Levels[0].Height;

    public byte[] PixelData { get => Levels[0].PixelData; }

    public Texture(string name, TextureFormat format, TextureLevel[] levels) : base(name)
    {
        Format = format;
        Levels = levels;
    }

    public Texture(string name, TextureFormat format, int width, int height, byte[] pixels) :
        this(name, format, new[] { new TextureLevel(width, height, pixels) })
    { }

    public static Texture CreatePlaceholder(string name)
    {
        return Checker;
    }

    public static Texture Checker;

    static Texture()
    {
        var pixels = new byte[16] {
            255, 0, 0, 255,
            0, 255, 0, 255,
            0, 0, 255, 255,
            0, 0, 0, 255
        };

        Checker = new Texture("Checker", TextureFormat.RGBA32, 2, 2, pixels);
    }
}

public record class TextureLevel(int Width, int Height, byte[] PixelData);

