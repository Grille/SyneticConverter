using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.LowLevel;

internal unsafe static class FixedStringUtils
{
    public static string MemToString<T>(T* ptr) where T : unmanaged
    {
        int size = sizeof(T);

        int trimsize = size;
        byte* byteptr = (byte*)ptr;
        for (trimsize = 0; trimsize < size; trimsize++)
            if (byteptr[trimsize] == 0) break;

        char[] chars = new char[trimsize];
        for (int i = 0; i < trimsize; i++)
            chars[i] = (char)byteptr[i];

        return new string(chars);
    }

    public static T StringToMem<T>(string d) where T : unmanaged
    {
        T str;
        int size = sizeof(T);
        byte* ptr = (byte*)&str;

        int length = Math.Min(d.Length, size);
        for (int i = 0; i < length; i++)
            ptr[i] = (byte)d[i];

        return str;
    }
}

[StructLayout(LayoutKind.Sequential, Size = 4)]
public struct String4
{
    public override string ToString() => (string)this;
    public static unsafe implicit operator string(String4 d) => FixedStringUtils.MemToString(&d);
    public static unsafe explicit operator String4(string d) => FixedStringUtils.StringToMem<String4>(d);

}

[StructLayout(LayoutKind.Sequential, Size = 8)]
public struct String8
{
    public override string ToString() => (string)this;
    public static unsafe implicit operator string(String8 d) => FixedStringUtils.MemToString(&d);
    public static unsafe explicit operator String8(string d) => FixedStringUtils.StringToMem<String8>(d);
}

[StructLayout(LayoutKind.Sequential, Size = 32)]
public struct String32
{
    public override string ToString() => (string)this;
    public static unsafe implicit operator string(String32 d) => FixedStringUtils.MemToString(&d);
    public static unsafe explicit operator String32(string d) => FixedStringUtils.StringToMem<String32>(d);
}

[StructLayout(LayoutKind.Sequential, Size = 48)]
public struct String48
{
    public override string ToString() => (string)this;
    public static unsafe implicit operator string(String48 d) => FixedStringUtils.MemToString(&d);
    public static unsafe explicit operator String48(string d) => FixedStringUtils.StringToMem<String48>(d);

}

[StructLayout(LayoutKind.Sequential, Size = 64)]
public struct String64
{
    public override string ToString() => (string)this;
    public static unsafe implicit operator string(String64 d) => FixedStringUtils.MemToString(&d);
    public static unsafe explicit operator String64(string d) => FixedStringUtils.StringToMem<String64>(d);
}
