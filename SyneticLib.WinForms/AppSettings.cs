using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DarkUI.Controls;
using DarkUI.Forms;

using SyneticLib.Files.Common;
using SyneticLib.Locations;

namespace SyneticLib.WinForms;

public static class AppSettings
{
    public static GameDirectoryList Games;

    public const string DefaultFileName = "config.ini";

    static AppSettings()
    {
        Games = new GameDirectoryList();
    }

    public static void Save(string filePath = DefaultFileName)
    {
        var iniFile = new AppSettingsFile();
        //iniFile.Games = Games;
        iniFile.Save(filePath);
    }

    public static void TryLoad(string filePath = DefaultFileName)
    {
        if (!File.Exists(filePath))
            return;

        Load(filePath);
    }

    public static void Load(string filePath = DefaultFileName)
    {
        var iniFile = new AppSettingsFile();
        iniFile.Load(filePath);

        //Games = iniFile.Games;

        if (Games == null)
        {

        }
    }

    public static void Setup()
    {
        TryLoad();

        if (Games.Count == 0)
        {
            SearchGamesDialog(null!);
        }
    }

    public static void SearchGamesDialog(IWin32Window owner)
    {
        var games = GameDirectoryList.Search();
        var newgames = Games.FilterNewLocations(games);

        var sb = new StringBuilder();


        sb.Append($"Found {games.Count} game locations.");
        if (newgames.Count != games.Count)
        {
            sb.Append($" ({newgames.Count} New)");
        }
        sb.AppendLine();

        if (newgames.Count > 0)
        {
            sb.AppendLine();
            foreach (var game in games)
            {
                sb.AppendLine($"{game.Version} {game.DirectoryPath}");
            }
        }
        var result = DarkMessageBox.ShowInformation(sb.ToString(), "Find Games", newgames.Count > 0 ? DarkDialogButton.OkCancel : DarkDialogButton.Close);
        if (result == DialogResult.OK)
        {
            foreach (var game in games)
            {
                Games.Add(game);
            }
        }
    }

}
