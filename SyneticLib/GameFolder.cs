using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib;

public  class GameFolder
{
    public string RootDir;
    public GameVersion Version;

    public Dictionary<string, Scenario> Scenarios = new();
    public Dictionary<string, Car> Cars = new();
    public Dictionary<string, RessourcePtr> Ressources = new();


    public GameFolder(string path, GameVersion target = GameVersion.Auto)
    {
        if (!Directory.Exists(path))
            throw new ArgumentException("invalid path");

        if (target == GameVersion.Invalid)
            throw new ArgumentException(target.ToString());

        RootDir = path;

        if (target == GameVersion.Auto)
            Version = GetGameVersion(path);
        else
            Version = target;
    }

    public Dictionary<string, Scenario> GetAllScenarios()
    {
        RefreshScenarios();

        return Scenarios;
    }

    public void Refresh()
    {
        RefreshScenarios();
        RefreshCars();
        RefreshRessources();
    }

    public void RefreshScenarios()
    {
        string scnRootDirPath = Path.Combine(RootDir, "Scenarios");
        if (!Directory.Exists(scnRootDirPath))
            return;

        var scnRootDir = Directory.GetDirectories(scnRootDirPath);

        Scenarios.Clear();
        foreach (var path in scnRootDir)
        {
            var name = Path.GetFileName(path);
            Scenarios.Add(name, GetScenario(name));
        }
    }

    public void RefreshCars()
    {
        var carRootDir = Directory.GetDirectories(Path.Combine(RootDir, "Autos"));

        Cars.Clear();
        foreach (var path in carRootDir)
        {
            var name = Path.GetFileName(path);
            Cars.Add(name, new Car());
        }
    }

    public void RefreshRessources()
    {

    }

    public Scenario GetScenario(string name)
    {
        var path = Path.Combine(RootDir, "Scenarios", name);
        return new Scenario(this, path);
    }

    public static GameVersion GetGameVersion(string path)
    {
        var files = Directory.GetFiles(path);
        var names = new List<string>();

        foreach (var file in files)
            names.Add(Path.GetFileName(file).ToLower());

        foreach (var name in names)
        {
            var result = name switch
            {
                "nice1.exe" => GameVersion.NICE,
                "breakneck.exe" => GameVersion.NICE2,
                "mbwr_pc.exe" => GameVersion.MBWR,
                "wr2_pc.exe" => GameVersion.WR2,
                "c11_pc.exe" => GameVersion.C11,
                "crashtime.exe" => GameVersion.CT1AP,
                "burningwheels.exe" => GameVersion.CT2BW,
                "ferrarivr.exe" => GameVersion.FVR,
                "highwaynights.exe" => GameVersion.CT3HN,
                "crashtime4.exe" => GameVersion.CT4TS,
                "crashtime5.exe" => GameVersion.CT5U,

                "bn_setup.exe" => GameVersion.NICE2,
                "wr_setup.exe" => GameVersion.MBWR,
                "wr2_setup.exe" => GameVersion.WR2,
                "c11_setup.exe" => GameVersion.C11,
                "ct_setup.exe" => GameVersion.CT1AP,
                "bw_setup.exe" => GameVersion.CT2BW,
                "fvr_setup.exe" => GameVersion.FVR,
                "hn_setup.exe" => GameVersion.CT3HN,
                "ct4_setup.exe" => GameVersion.CT4TS,
                "ct5_config.exe" => GameVersion.CT5U,

                _ => GameVersion.Invalid,
            };

            if (result != GameVersion.Invalid)
                return result;
        }

        return GameVersion.Invalid;
    }

}
