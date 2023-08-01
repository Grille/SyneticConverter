using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.IO.Synetic;

namespace SyneticLib;
public class Texture : Ressource
{
    public TextureFormat Format;
    public int Width;
    public int Height;

    public TextureLevel[] Levels;
    public byte[] PixelData { get => Levels[0].PixelData; }

    public int Id;

    public Texture(int width, int height) : base(null, "Texture", PointerType.Virtual)
    {
        Format = TextureFormat.Rgba32;

        Width = width;
        Height = height;

        Levels = new TextureLevel[1] { new() };
        Levels[0].PixelData = new byte[width * height * 4];
    }

    public Texture() : base(null, "Texture", PointerType.Virtual)
    {
        Format = TextureFormat.Rgba32;

        Width = 2;
        Height = 2;

        Levels = new TextureLevel[1] { new() };
        Levels[0].PixelData = new byte[16] {
            255, 0, 0, 255,
            0, 255, 0, 255,
            0, 0, 255, 255,
            0, 0, 0, 255
        };

        DataState = DataState.Loaded;
    }

    public Texture(Ressource parent, string path) : base(parent, path, PointerType.File)
    {
    }

    public static Texture CreatePlaceholder(string name)
    {
        var texture = new Texture();
        texture.SourcePath = name;
        return texture;
    }

    /*
    public void CopyPixelData(byte[] pixels, int width, int height, int offset = 0)
    {
        byte[] data = new byte[width * height * format.Stride];
        Array.Copy(pixels, offset, data, 0, data.Length);
        PixelDataPtr(data, width, height, format);
    }
    */

    public void PixelDataPtr(int width, int height, byte[] pixels)
    {
        //PixelData = pixels;
        Width = width;
        Height = height;
    }

    public void LoadFromPtx()
    {
        new TextureImporterPtx(this).Load();
    }

    protected override void OnLoad()
    {
        LoadFromPtx();
    }

    protected override void OnSave()
    {

    }

    protected override void OnSeek()
    {
        //throw new NotImplementedException();
    }

    public static Texture Checker;

    static Texture()
    {
        Checker = new();
        //Checker.CopyPixelData(new int[] { 0xFFF})
    }
}

public class TextureLevel
{
    public byte[] PixelData;
}
