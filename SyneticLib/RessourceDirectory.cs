using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class RessourceDirectory<T> : RessourceList<T> where T : Ressource
{
    public Predicate<string> Filter;
    public Func<string, T> Constructor;

    public RessourceDirectory(Ressource parent, string path) : base(parent, PointerType.Directory)
    {
        SourcePath = path;
        Items = new();
    }

    protected override void OnSeek()
    {
        var files = Directory.GetFiles(SourcePath);

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
