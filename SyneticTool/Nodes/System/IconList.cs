using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;

namespace SyneticTool.Nodes.System;
internal static class IconList
{
    public static int Default;
    public static int Terrain;
    public static int World;
    public static int Audio;
    public static int Car;
    public static int Texture;
    public static int Mesh;
    public static int Misc;

    public static int NICE;
    public static int NICE2;

    public static int MBTR;
    public static int MBWR;
    public static int WR2;
    public static int C11;
    public static int CT1;
    public static int CT2;
    public static int CT3;
    public static int CT4;
    public static int CT5;
    public static int FVR;

    public readonly static ImageList Images;


    static IconList()
    {
        Images = new ImageList();
        Default = LoadIcon("Line");
        Terrain = LoadIcon("Terrain");
        World = LoadIcon("World");
        Audio = LoadIcon("Audio");
        Car = LoadIcon("Car");
        Texture = LoadIcon("Texture");
        Mesh = LoadIcon("Mesh");
        Misc = LoadIcon("Misc");

        NICE = LoadIcon("NICE");
        NICE2 = LoadIcon("NICE");

        MBTR = LoadIcon("MBWR");
        MBWR = LoadIcon("MBWR");
        WR2 = LoadIcon("WR2");
        C11 = LoadIcon("C11");
        CT1 = LoadIcon("CT1");
        CT2 = LoadIcon("CT2");
        CT3 = LoadIcon("CT3");
        CT4 = LoadIcon("CT4");
        CT5 = LoadIcon("CT5");
        FVR = LoadIcon("FVR");
    }

    static int LoadIcon(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"SyneticTool.Nodes.Icons.{name}.png";
        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            Images.Images.Add(new Bitmap(stream));
        return Images.Images.Count - 1;
    }




}
