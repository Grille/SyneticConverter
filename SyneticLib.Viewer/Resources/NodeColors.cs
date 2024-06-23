using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SyneticLib.WinForms.Resources;

internal static class NodeColors
{
    public static readonly Color Default = Color.Black;
    public static readonly Color Virtual = Color.Blue;
    public static readonly Color Changed = Color.Green;
    public static readonly Color Failed = Color.Red;

    public static Color RessourceColor(SyneticObject file) => Default;
}
