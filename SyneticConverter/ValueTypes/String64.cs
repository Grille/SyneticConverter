using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SyneticConverter;

[StructLayout(LayoutKind.Sequential, Size = 64)]
public struct String64
{
    public unsafe override string ToString()
    {
        return (string)this;
    }
    public static unsafe implicit operator string(String64 d) => new string((sbyte*)&d, 0, 64, Encoding.ASCII).Trim((char)0);
}
