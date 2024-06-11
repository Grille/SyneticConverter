using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;

using static FixedStringUtils;

file unsafe static class FixedStringUtils
{
    readonly static Encoding encoding = Encoding.UTF8;

    public static string MemToString<T>(T* ptr) where T : unmanaged
    {
        int size = sizeof(T);
        byte* byteptr = (byte*)ptr;

        int length;
        for (length = 0; length < size; length++)
            if (byteptr[length] == 0) break;

        return encoding.GetString(byteptr, length); ;
    }

    public static T StringToMem<T>(string d) where T : unmanaged
    {
        int size = sizeof(T);

        if (d.Length > size)
            throw new ArgumentException($"String.Length '{d.Length}' must be <= '{size}'.", nameof(d));

        T strobj;
        var span = new Span<byte>(&strobj, size);

        encoding.GetBytes(d.AsSpan(), span);

        return strobj;
    }
}

[StructLayout(LayoutKind.Sequential, Size = 4)]
public struct String4
{
    public override string ToString() => (string)this;
    public static unsafe implicit operator string(String4 d) => MemToString(&d);
    public static unsafe explicit operator String4(string d) => StringToMem<String4>(d);
}

[StructLayout(LayoutKind.Sequential, Size = 8)]
public struct String8
{
    public override string ToString() => (string)this;
    public static unsafe implicit operator string(String8 d) => MemToString(&d);
    public static unsafe explicit operator String8(string d) => StringToMem<String8>(d);
}

[StructLayout(LayoutKind.Sequential, Size = 16)]
public struct String16
{
    public override string ToString() => (string)this;
    public static unsafe implicit operator string(String16 d) => MemToString(&d);
    public static unsafe explicit operator String16(string d) => StringToMem<String16>(d);
}

[StructLayout(LayoutKind.Sequential, Size = 32)]
public struct String32
{
    public override string ToString() => (string)this;
    public static unsafe implicit operator string(String32 d) => MemToString(&d);
    public static unsafe explicit operator String32(string d) => StringToMem<String32>(d);
}

[StructLayout(LayoutKind.Sequential, Size = 48)]
public struct String48
{
    public override string ToString() => (string)this;
    public static unsafe implicit operator string(String48 d) => MemToString(&d);
    public static unsafe explicit operator String48(string d) => StringToMem<String48>(d);

}

[StructLayout(LayoutKind.Sequential, Size = 64)]
public struct String64
{
    public override string ToString() => (string)this;
    public static unsafe implicit operator string(String64 d) => MemToString(&d);
    public static unsafe explicit operator String64(string d) => StringToMem<String64>(d);
}
