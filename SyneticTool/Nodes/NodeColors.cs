using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using SyneticLib;

namespace SyneticTool.Nodes;

internal static class NodeColors
{
    public static readonly Color Default = Color.Black;
    public static readonly Color Pointer = Color.Blue;
    public static readonly Color Changed = Color.Green;
    public static readonly Color Failed = Color.Red;

    public static Color RessourceColor(RessourceFile file) => (file.PointerState, file.DataState) switch
    {
        (PointerState.Exists, DataState.None) => Pointer,
        (PointerState.Exists, DataState.Loaded) => Default,
        (PointerState.Exists, DataState.Changed) => Changed,
        (_, DataState.Loaded) => Changed,
        (_, DataState.Changed) => Changed,
        (_, _) => Failed,
    };

    public static Color RessourceColor(Ressource file) => (file.PointerState, file.DataState) switch
    {
        (PointerState.Exists, DataState.None) => Pointer,
        (PointerState.Exists, DataState.Loaded) => Default,
        (PointerState.Exists, DataState.Changed) => Changed,
        (_, DataState.Loaded) => Changed,
        (_, DataState.Changed) => Changed,
        (_, _) => Failed,
    };
}
