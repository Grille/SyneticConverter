using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Collections;

internal class IndexDict<T>
{
    Dictionary<T, int> dict;

    public IndexDict()
    {
        dict = new();
    }
     
    public int GetIndex(T obj)
    {
        if (dict.TryGetValue(obj, out var index))
        {
            return index;
        }
        index = dict.Count;
        dict.Add(obj, index);
        return index;
    }
}
