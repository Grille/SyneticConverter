using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class Texture : Ressource
{
    public TextureFormat Format { get; }

    public TextureLevel[] Levels { get; }

    public int Width => Levels[0].Width;

    public int Height => Levels[0].Height;

    public byte[] PixelData { get => Levels[0].Data; }

    public Texture(string name, TextureFormat format, TextureLevel[] levels) : base(name)
    {
        Format = format;
        Levels = levels;

        if (levels.Length == 0)
            throw new ArgumentException("", nameof(levels));
    }

    public Texture(string name, TextureFormat format, int width, int height, byte[] pixels) :
        this(name, format, new[] { new TextureLevel(width, height, pixels) })
    { }

    public static Texture CreatePlaceholder(string name)
    {
        return new Texture(name,Textures.Error.Format,Textures.Error.Levels);
    }
}

public unsafe class TextureLevel
{
    public int Width { get; }

    public int Height { get; }

    public byte[] Data { get; }

    bool locked = false;
    GCHandle handle;

    public nint Scan0
    {
        get
        {
            AssertLock(true);

            return handle.AddrOfPinnedObject();
        }
    }

    public TextureLevel(int width, int height, byte[] data)
    {
        Width = width; 
        Height = height;
        Data = data;
    }

    public void Lock()
    {
        AssertLock(false);

        locked = true;
        handle = GCHandle.Alloc(Data, GCHandleType.Pinned);
    }

    public void Unlock()
    {
        AssertLock(true);

        handle.Free();
        locked = false;
    }

    void AssertLock(bool expected)
    {
        if (locked != expected)
        {
            if (expected)
            {
                throw new InvalidOperationException("TextureLevel needs to be locked");
            }
            throw new InvalidOperationException("TextureLevel alredy locked");
        }
    }
}

