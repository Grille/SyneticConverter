using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using SyneticLib.Resources;

namespace SyneticLib.WinForms.Resources;
public static class EmbeddedImageList
{
    public readonly static ImageHandle Default;
    public readonly static ImageHandle Terrain;
    public readonly static ImageHandle World;
    public readonly static ImageHandle Audio;
    public readonly static ImageHandle Car;
    public readonly static ImageHandle Texture;
    public readonly static ImageHandle Mesh;
    public readonly static ImageHandle Misc;

    public readonly static ImageHandle NICE;
    public readonly static ImageHandle NICE2;
    public readonly static ImageHandle MBTR;
    public readonly static ImageHandle MBWR;
    public readonly static ImageHandle WR2;
    public readonly static ImageHandle C11;
    public readonly static ImageHandle CT1;
    public readonly static ImageHandle CT2;
    public readonly static ImageHandle CT3;
    public readonly static ImageHandle CT4;
    public readonly static ImageHandle CT5;
    public readonly static ImageHandle FVR;

    public readonly static ImageHandle SyneticLib;

    public readonly static ImageList ImageList;

    static EmbeddedImageList()
    {
        ImageList = new ImageList();

        Default = LoadImage("Line");
        Terrain = LoadImage("Terrain");
        World = LoadImage("World");
        Audio = LoadImage("Audio");
        Car = LoadImage("Car");
        Texture = LoadImage("Texture");
        Mesh = LoadImage("Mesh");
        Misc = LoadImage("Misc");

        NICE = LoadImage(GameIcons.NICE);
        NICE2 = LoadImage(GameIcons.NICE2);
        MBTR = LoadImage(GameIcons.MBTR);
        MBWR = LoadImage(GameIcons.MBWR);
        WR2 = LoadImage(GameIcons.WR2);
        C11 = LoadImage(GameIcons.C11);
        CT1 = LoadImage(GameIcons.CT1);
        CT2 = LoadImage(GameIcons.CT2);
        CT3 = LoadImage(GameIcons.CT3);
        CT4 = LoadImage(GameIcons.CT4);
        CT5 = LoadImage(GameIcons.CT5);
        FVR = LoadImage(GameIcons.FVR);

        SyneticLib = LoadImage(GameIcons.SyneticLib);
    }

    public record class ImageHandle
    {
        public Bitmap Bitmap { get; }

        public Bitmap Bitmap16 { get; }
        public Icon Icon { get; }
        public int Index { get; }

        public ImageHandle(Stream stream)
        {
            Index = ImageList.Images.Count;

            Bitmap = new Bitmap(stream);
            ImageList.Images.Add(Bitmap);

            Icon = Icon.FromHandle(Bitmap.GetHicon());

            Bitmap16 = new Bitmap(Bitmap, new Size(16, 16));
        }
    }

    static ImageHandle LoadImage(Stream stream, bool leaveOpen = false)
    {
        try
        {
            return new ImageHandle(stream);
        }
        finally
        {
            if (!leaveOpen)
            {
                stream.Dispose();
            }
        }
    }

    static ImageHandle LoadImage(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"SyneticLib.WinForms.Resources.Assets.{name}.png";
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            var names = assembly.GetManifestResourceNames();
            throw new FileNotFoundException(resourceName);
        }

        return new ImageHandle(stream);
    }
}
