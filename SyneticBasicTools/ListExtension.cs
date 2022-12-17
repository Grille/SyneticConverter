using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticBasicTools;

internal static class ListExtension
{
    public static void InsertAfter<T>(this List<T> list, T item, T nitem)
    {
        int idx = list.IndexOf(item);
        if (idx != -1)
            list.Insert(idx + 1, nitem);
        else
            list.Add(nitem);
    }
    public static void UpItem<T>(this List<T> list, T item)
    {
        int idx = list.IndexOf(item);
        if (idx > 0)
            list.Swap(idx, idx - 1);
    }

    public static void DownItem<T>(this List<T> list, T item)
    {
        int idx = list.IndexOf(item);
        if (idx < list.Count - 1)
            list.Swap(idx, idx + 1);
    }

    public static void Swap<T>(this List<T> list, int idx1, int idx2)
    {
        var item1 = list[idx1];
        list[idx1] = list[idx2];
        list[idx2] = item1;
    }
}

