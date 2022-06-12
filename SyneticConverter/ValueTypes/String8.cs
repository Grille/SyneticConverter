using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SyneticConverter;

[StructLayout(LayoutKind.Sequential, Size = 8)]
public struct String8
{
    public unsafe override string ToString()
    {
        return (string)this;
    }
    public static unsafe implicit operator string(String8 d) => new string((sbyte*)&d, 0, 8, Encoding.ASCII).Trim((char)0);
}
