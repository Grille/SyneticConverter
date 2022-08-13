using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib;

namespace SyneticTool;

public class Games
{
    public Dictionary<string, GameFolder> GameFolders = new();

    public bool Exists(string name)
    {
        return GameFolders.ContainsKey(name);
    }

    public GameFolder CreateGame(string name, string path, GameVersion version = GameVersion.Auto)
    {
        GameFolder folder = new GameFolder(path, version);
        GameFolders.Add(name, new GameFolder(path, version));
        return folder;
    }

    public void RemoveGame(string name)
    {
        GameFolders.Remove(name);
    }
}
