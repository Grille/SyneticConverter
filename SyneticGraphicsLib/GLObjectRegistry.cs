using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Graphics;

public class GLObjectRegistry<TKey, TValue> : IReadOnlyCollection<TValue> where TKey : Ressource where TValue : GLStateObject
{
    readonly Dictionary<TKey, TValue> pairs;
    readonly Func<TKey, TValue> mapper;

    public int Count => pairs.Count;

    public GLObjectRegistry(Func<TKey, TValue> mapper)
    {
        this.mapper = mapper;
    }

    public TValue Get(TKey ressource)
    {
        if (pairs.TryGetValue(ressource, out TValue buffer))
        {
            return buffer;
        }

        buffer = mapper(ressource);
        pairs.Add(ressource, buffer);
        return buffer;
    }

    public void Clear()
    {
        foreach (var buffer in pairs.Values)
        {
            buffer.Dispose();
        }
        pairs.Clear();
    }

    public IEnumerator<TValue> GetEnumerator()
    {
        return pairs.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return pairs.Values.GetEnumerator();
    }
}
