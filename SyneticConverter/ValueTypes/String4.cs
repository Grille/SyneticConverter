using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SyneticConverter;

[StructLayout(LayoutKind.Sequential, Size = 4)]
public struct String4
{
    public unsafe override string ToString()
    {
        return (string)this;
    }
    public static unsafe implicit operator string(String4 d) => new string((sbyte*)&d, 0, 4, Encoding.ASCII).Trim((char)0);
}
