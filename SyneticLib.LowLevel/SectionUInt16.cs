using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SectionUInt16
{
    public ushort Start;
    public ushort Length;

    public int End => Start + Length;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SectionInt32
{
    public int Start;
    public int Length;

    public int End => Start + Length;
}
