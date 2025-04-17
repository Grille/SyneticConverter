using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.IO.Generic;
public class SerializerRegistry<T> : IEnumerable<ISerializer<T>>
{
    private readonly Dictionary<string, ISerializer<T>> _dict = new();

    public Dictionary<string, ISerializer<T>>.KeyCollection Keys => _dict.Keys;

    public void Add(string key, ISerializer<T> obj)
    {
        _dict.Add(GetKey(key), obj);
    }

    public ISerializer<T> this[string key]
    {
        get => _dict[GetKey(key)];
    }

    string GetKey(string key)
    {
        var lkey = key.ToLowerInvariant();
        return lkey[0] == '.' ? lkey.Substring(1) : lkey;
    }

    public T Load(string path, string? key = null)
    {
        if (key == null) key = Path.GetExtension(path);
        var serializer = this[key];
        return serializer.Load(path);
    }

    public void Save(string path, T obj, string? key = null)
    {
        if (key == null) key = Path.GetExtension(path);
        var serializer = this[key];
        serializer.Save(path, obj);
    }

    public IEnumerator<ISerializer<T>> GetEnumerator()
    {
        return _dict.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _dict.Values.GetEnumerator();
    }
}
