using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Graphics;

namespace SyneticLib;
public class Texture : Ressource
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Stride { get; private set; }
    public byte[] PixelData { get; private set; }

    public PixelAttrPtr PixelFormat { get; private set; }

    public int Id;

    public readonly TextureBuffer GLBuffer;

    public Texture(Ressource parent, string path) : base(parent, path, PointerType.File)
    {
        GLBuffer = new TextureBuffer(this);
    }

    public void CopyPixelData(byte[] pixels, int width, int height, PixelAttrPtr format, int offset = 0)
    {
        byte[] data = new byte[width * height * format.Stride];
        Array.Copy(pixels, offset, data, 0, data.Length);
        PixelDataPtr(data, width, height, format);
    }

    public void PixelDataPtr(byte[] pixels, int width, int height, PixelAttrPtr format)
    {
        PixelData = pixels;
        Width = width;
        Height = height;
        PixelFormat = format;
    }

    protected override void OnLoad()
    {

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
        Checker = new(null, "checker");
        Checker.PointerType = PointerType.Virtual;
        //Checker.CopyPixelData(new int[] { 0xFFF})
    }
}
