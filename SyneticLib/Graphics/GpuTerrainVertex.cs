using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Runtime.InteropServices;

namespace SyneticLib.Graphics;

[StructLayout(LayoutKind.Explicit, Size = 32)]
internal unsafe struct GpuTerrainVertex
{
    public const int LocationPosition = 0;
    public const int LocationDebugColor = LocationPosition + 12;

    [FieldOffset(LocationPosition)]
    public Vector3 Position;

    [FieldOffset(LocationDebugColor)]
    public Vector3 DebugColor;
}
