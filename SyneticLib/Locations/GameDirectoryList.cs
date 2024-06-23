using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Locations;
public class GameDirectoryList : List<GameDirectory>
{
    public GameDirectoryList() : base() { }

    public List<GameDirectory> FindByPath(string path)
    {
        var fullpath = Path.GetFullPath(path).ToLower();
        return FindAll((a) => Path.GetFullPath(a.DirectoryPath).ToLower() == fullpath);
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

    public GameDirectoryList FindNewGames()
    {
        var locations = new string[]
{
            "",
            "Games",
            "Programs",
            "Programme",
            "Program Files",
            "Program Files (x86)",
            "Program Files\\steamapps\\steamapps\\common",
            "Program Files (x86)\\Steam\\steamapps\\common",
        };
        return FindNewGames(locations);
    }

    public GameDirectoryList FindNewGames(string[] locations)
    {
        var res = new GameDirectoryList();

        var names = new string[]
        {
            "", "TDK", "Synetic"
        };

        var drives = DriveInfo.GetDrives();


        foreach (var drive in drives)
        {
            foreach (var location in locations)
            {
                var path0 = Path.Join(drive.Name, location);
                if (!Directory.Exists(path0))
                    continue;

                foreach (var name in names)
                {
                    var path = Path.Join(path0, name);
                    if (!Directory.Exists(path))
                        continue;

                    string[] directory;
                    try
                    {
                        directory = Directory.GetDirectories(path);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        continue;
                    }
                    foreach (var fpath in directory)
                    {
                        GameVersion version;
                        try
                        {
                            version = GameDirectory.GetDirectoryGameVersion(fpath);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            continue;
                        }
                        if (version != GameVersion.None)
                        {
                            if (!PathExists(fpath))
                            {
                                res.Add(new(fpath, version));
                            }
                        }
                    }
                }
            }
        }

        return res;
    }
}
