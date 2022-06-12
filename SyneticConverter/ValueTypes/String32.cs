using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SyneticConverter;

[StructLayout(LayoutKind.Sequential, Size = 32)]
public struct String32
{
    public unsafe override string ToString()
    {
        return (string)this;
    }
    public static unsafe implicit operator string(String32 d) => new string((sbyte*)&d, 0, 32, Encoding.ASCII).Trim((char)0);
}
