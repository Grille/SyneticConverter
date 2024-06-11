using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using SyneticLib.Resources;

namespace SyneticLib.Viewer;

public static class Icons
{
    public static Bitmap SyneticLib { get; } = new Bitmap(GameIcons.SyneticLib);

    public static Bitmap MBWR { get; } = new Bitmap(GameIcons.MBWR);

    public static Bitmap WR2 { get; } = new Bitmap(GameIcons.WR2);

    public static Bitmap C11 { get; } = new Bitmap(GameIcons.C11);
}
