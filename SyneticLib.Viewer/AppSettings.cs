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
        iniFile.Games = Games;
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

        Games = iniFile.Games;

        if (Games == null)
        {

        }
    }

    public static void Setup()
    {
        TryLoad();

        if (Games.Count == 0)
        {
            var newgames = Games.FindNewGames();
            ShowApplyGamesDialog(null!, newgames);
        }
    }

    public static void ShowApplyGamesDialog(IWin32Window owner, List<GameDirectory> games)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Found {games.Count} new game locations.");
        if (games.Count > 0)
        {
            sb.AppendLine();
            foreach (var game in games)
            {
                sb.AppendLine($"{game.Version} {game.DirectoryPath}");
            }
        }
        var result = DarkMessageBox.ShowInformation(sb.ToString(), "Find Games", DarkDialogButton.OkCancel);
        if (result == DialogResult.OK)
        {
            foreach (var game in games)
            {
                Games.Add(game);
            }
        }
    }

}
