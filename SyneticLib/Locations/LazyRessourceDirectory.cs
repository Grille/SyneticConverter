﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.IO.Path;

namespace SyneticLib.Locations;

public class LazyRessourceDirectory<T> : DirectoryLocation, IReadOnlyCollection<Lazy<T>> where T : SyneticObject
{
    public static readonly Predicate<string> FileFilter = (path) => File.Exists(path);
    public static readonly Predicate<string> DirectoryFilter = (path) => Directory.Exists(path);

    readonly Predicate<string> Filter;
    readonly Func<string, T> Constructor;

    readonly Dictionary<string, Lazy<T>> dict;

    public LazyRessourceDirectory(string path, Predicate<string> filter, Func<string, T> constructor) : base(path)
    {
        dict = new Dictionary<string, Lazy<T>>();
        Filter = filter;
        Constructor = constructor;

        Seek();
    }

    public int Count => dict.Count;

    protected override void OnSeek()
    {
        if (!Directory.Exists(DirectoryPath))
            return;

        var entries = Directory.GetFileSystemEntries(DirectoryPath);

        dict.Clear();
        foreach (var entry in entries)
        {
            if (Filter(entry))
            {
                var path = GetFullPath(entry).ToLower();
                var factory = () => Constructor(path);
                dict[path] = new Lazy<T>(factory); ;
            }
        }
    }

    public bool TryGetByKey(string name, out T result)
    {
        var key = GetPath(name);


        if (dict.TryGetValue(key, out var lazy))
        {
            result = lazy.Value;
            return true;
        }
        result = null!;
        return false;
    }

    public string GetKey(string path)
    {
        return GetFileNameWithoutExtension(path).ToLower();
    }

    public string GetPath(string key)
    {
        var ckey = GetKey(key);
        return dict.Keys.First((path) => GetKey(path) == ckey);
        //return Combine(Path, key);
    }

    public T GetByFileName(string name)
    {
        if (TryGetByKey(name, out var result))
            return result;

        throw new KeyNotFoundException(name);
    }

    public T[] CreateIndexedArray(IList names)
    {
        var result = new T[names.Count];
        for (var i = 0; i < names.Count; i++)
        {
            var name = names[i]!.ToString();
            ArgumentNullException.ThrowIfNull(name);
            result[i] = GetByFileName(name);
        }
        return result;
    }

    public T[] CreateIndexedArray(string[] names)
    {
        return CreateIndexedArray(names);
    }

    public IEnumerator<Lazy<T>> GetEnumerator()
    {
        return dict.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return dict.Values.GetEnumerator();
    }
}
