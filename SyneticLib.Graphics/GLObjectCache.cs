using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Graphics;

public class GLObjectCache<TKey, TValue> : IReadOnlyCollection<TValue> where TKey : Ressource where TValue : GLObject
{
    readonly Dictionary<int, TValue> pairs;
    readonly Func<TKey, TValue> mapper;

    public int Count => pairs.Count;

    public GLObjectCache(Func<TKey, TValue> mapper)
    {
        pairs = new Dictionary<int, TValue>();
        this.mapper = mapper;
    }

    public TValue Get(TKey ressource)
    {
        int id = ressource.RessourceID;

        if (pairs.TryGetValue(id, out TValue buffer))
        {
            return buffer;
        }

        buffer = mapper(ressource);
        pairs.Add(id, buffer);
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
