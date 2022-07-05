using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticConverter;

namespace SyneticTool;

public class Games
{
    public Dictionary<string, GameFolder> GameFolders = new();

    public void AddGame(string name, string path, GameVersion version = GameVersion.Auto)
    {
        GameFolders.Add(name, new GameFolder(path, version));
    }

    public void RemoveGame(string name)
    {
        GameFolders.Remove(name);
    }
}
