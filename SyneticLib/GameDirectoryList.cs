using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class GameDirectoryList : List<GameDirectory>
{
    public GameDirectoryList() : base() { }

    public List<GameDirectory> FindByPath(string path)
    {
        string fullpath = Path.GetFullPath(path).ToLower();
        return FindAll((a) => Path.GetFullPath(a.SourcePath).ToLower() == fullpath);
    }

    public bool PathExists(string path)
    {
        var items = FindByPath(path);
        return items.Count > 0;
    }

    public GameDirectory GetOrCreateEntry(string path)
    {
        var items = FindByPath(path);
        if (items.Count > 1)
            throw new Exception("nope");

        if (items.Count == 0)
        {
            return new GameDirectory(path);
        }
        else
        {
            return items[0];
        }

    }
}
