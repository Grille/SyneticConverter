using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Grille.IO;

namespace SyneticLib;

internal static class BinaryUtils
{
    public static TTo[] CastArray<TFrom, TTo>(TFrom[] array, Func<TFrom, TTo> cast)
    {
        var result = new TTo[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            result[i] = cast(array[i]);
        }
        return result;
    }

    public static unsafe void ReadItemBytesToArray<T>(this BinaryViewReader br, T[] array, int itemByteSize) where T : unmanaged
    {
        int size = sizeof(T);
        if (size < itemByteSize)
            throw new ArgumentException();

        for (int i = 0; i < array.Length; i++)
        {
            var obj = new T();
            br.ReadToPtr(&obj, itemByteSize);
            array[i] = obj;
        }
    }

    public static unsafe void WriteItemBytesFromArray<T>(this BinaryViewWriter bw, T[] array, int itemByteSize) where T : unmanaged
    {
        int size = sizeof(T);
        if (size < itemByteSize)
            throw new ArgumentException();

        for (int i = 0; i < array.Length; i++)
        {
            var obj = array[i];
            bw.WriteFromPtr(&obj, itemByteSize);
        }
    }


    public static void ReadToArray<TFrom, TTo>(this BinaryViewReader br, TTo[] array, Func<TFrom, TTo> cast) where TFrom : unmanaged
    {
        for (int i = 0; i < array.Length; i++)
        {
            var obj = br.Read<TFrom>();
            array[i] = cast(obj);
        }
    }

    public static void WriteArray<TFrom, TTo>(this BinaryViewWriter bw, TFrom[] array, Func<TFrom, TTo> cast) where TTo : unmanaged
    {
        for (int i = 0; i < array.Length; i++)
        {
            bw.Write(cast(array[i]));
        }
    }
}
