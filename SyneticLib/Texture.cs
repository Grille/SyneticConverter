using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

using SyneticLib.Utils;

namespace SyneticLib;
public class Texture : SyneticObject
{
    public TextureFormat Format { get; }

    public TextureLevel[] Levels { get; }

    public int Width => Levels[0].Width;

    public int Height => Levels[0].Height;

    public Vector2 Size
    {
        get => new Vector2(Width, Height);
    }

    public byte[] MainSurfaceData { get => Levels[0].Data; }

    public Texture(TextureFormat format, TextureLevel[] levels) : base()
    {
        Format = format;
        Levels = (TextureLevel[])levels.Clone();

        if (levels.Length == 0)
        {
            throw new ArgumentException("levels.Length == 0", nameof(levels));
        }

        int width = Levels[0].Width;
        int height = Levels[0].Height;

        for (int i = 0; i < Levels.Length; i++)
        {
            if (Levels[i].Width != width || Levels[i].Height != height)
            {
                throw new ArgumentException("Every level must be half the size of its parent.", nameof(levels));
            }

            width /= 2;
            height /= 2;
        }
    }

    public Texture(TextureFormat format, int width, int height, byte[] pixels) :
        this(format, new[] { new TextureLevel(width, height, pixels) })
    { }

    public Texture(TextureFormat format, TextureLevel level) :
        this(format, new[] { level })
    { }

    public static Texture CreatePlaceholder(string name)
    {
        throw new NotImplementedException();
    }
}
