using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.IO.Path;

namespace SyneticLib.Locations;

public class RessourceDirectory<T> : Location, IReadOnlyCollection<T> where T : Ressource
{
    public static readonly Predicate<string> FileFilter = (path) => File.Exists(path);
    public static readonly Predicate<string> DirectoryFilter = (path) => Directory.Exists(path);

    readonly Predicate<string> Filter;
    readonly Func<string, T> Constructor;

    readonly Dictionary<string, T> dict;

    public RessourceDirectory(string path, Predicate<string> filter, Func<string, T> constructor) : base(path)
    {
        dict = new Dictionary<string, T>();
        Filter = filter;
        Constructor = constructor;

        Seek();
    }

    public int Count => dict.Count;

    protected override void OnSeek()
    {
        var entries = Directory.GetFileSystemEntries(Path);

        dict.Clear();
        foreach (var entry in entries)
        {
            if (Filter(entry))
            {
                var key = GetFullPath(entry).ToLower();
                dict[key] = null;
            }
        }
    }

    public bool TryGetByFileName(string name, out T result)
    {
        var key = GetFullPath(Combine(Path, name)).ToLower();

        if (dict.TryGetValue(key, out result))
        {
            if (result == null)
            {
                result = Constructor(key);
                dict[key] = result;
            }
            return true;
        }
        return false;
    }

    public T GetByFileName(string name)
    {
        if (TryGetByFileName(name, out var result))
            return result;

        throw new KeyNotFoundException(name);
    }

    public T[] CreateIndexedArray<TList>(TList names) where TList : IList
    {
        var result = new T[names.Count];
        for (var i = 0; i < names.Count; i++)
        {
            result[i] = GetByFileName(names[i].ToString());
        }
        return result;
    }

    public T[] CreateIndexedArray(string[] names)
    {
        return CreateIndexedArray(names);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return dict.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return dict.Values.GetEnumerator();
    }
}
