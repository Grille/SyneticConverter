using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;

[StructLayout(LayoutKind.Sequential, Size = 4)]
public struct String4
{
    const int size = 4;

    public unsafe override string ToString()
    {
        return (string)this;
    }
    public static unsafe implicit operator string(String4 d) => new string((sbyte*)&d, 0, size, Encoding.ASCII).Trim((char)0);
    public static unsafe explicit operator String4(string d)
    {
        String4 str;
        byte* ptr = (byte*)&str;

        int length = Math.Min(d.Length, size);
        for (int i = 0; i < length; i++)
        {
            ptr[i] = (byte)d[i];
        }
        return str;
    }

}

[StructLayout(LayoutKind.Sequential, Size = 8)]
public struct String8
{
    public unsafe override string ToString()
    {
        return (string)this;
    }
    public static unsafe implicit operator string(String8 d) => new string((sbyte*)&d, 0, 8, Encoding.ASCII).Trim((char)0);
}

[StructLayout(LayoutKind.Sequential, Size = 32)]
public struct String32
{
    public unsafe override string ToString()
    {
        return (string)this;
    }
    public static unsafe implicit operator string(String32 d) => new string((sbyte*)&d, 0, 32, Encoding.ASCII).Trim((char)0);
}

[StructLayout(LayoutKind.Sequential, Size = 48)]
public struct String48
{
    public unsafe override string ToString()
    {
        return (string)this;
    }
    public static unsafe implicit operator string(String48 d) => new string((sbyte*)&d, 0, 48, Encoding.ASCII).Trim((char)0);
}

[StructLayout(LayoutKind.Sequential, Size = 64)]
public struct String64
{
    public unsafe override string ToString()
    {
        return (string)this;
    }
    public static unsafe implicit operator string(String64 d) => new string((sbyte*)&d, 0, 64, Encoding.ASCII).Trim((char)0);
}

