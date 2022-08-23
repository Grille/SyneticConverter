﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class RessourceDirectory<T> : RessourceList<T> where T : Ressource
{
    public static readonly Predicate<string> FileFilter = (path) => File.Exists(path);
    public static readonly Predicate<string> DirectoryFilter = (path) => Directory.Exists(path);

    public Predicate<string> Filter;
    public Func<string, T> Constructor;

    public ProgressInfo Progress;

    public RessourceDirectory(Ressource parent, string path, Predicate<string> filter, Func<string, T> constructor) : base(parent, PointerType.Directory)
    {
        SourcePath = path;
        Items = new();
        Filter = filter;
        Constructor = constructor;
        Progress = new ProgressInfo();
    }

    protected override void OnSeek()
    {
        var files = Directory.GetFileSystemEntries(SourcePath);

        Progress.Value = 0;

        Items.Clear();
        foreach (var file in files)
        {
            if (Filter(file))
            {
                var item = Constructor(file);
                Items.Add(item);
            }
        }
    }
}
