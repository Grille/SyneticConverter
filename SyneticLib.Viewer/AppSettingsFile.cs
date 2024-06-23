using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files.Common;
using SyneticLib.Locations;

namespace SyneticLib.WinForms;

public class AppSettingsFile : SyneticIniFile
{
    public GameDirectoryList Games { get; set; }

    public AppSettingsFile()
    {
        Games = new GameDirectoryList();
    }

    protected override void OnRead()
    {
        Games.Clear();

        foreach (var section in Sections)
        {
            if (section.Name == "GameLocation")
            {
                var location = section["Location"];
                var Version = Enum.Parse<GameVersion>(section["Version"], true);
                Games.Add(new GameDirectory(location, Version));
            }
        }
    }

    protected override void OnWrite()
    {
        foreach (var game in Games)
        {
            var section = new Section("GameLocation");
            section["Location"] = game.DirectoryPath;
            section["Version"] = game.Version.ToString();
            Sections.Add(section);

        }
    }


}
