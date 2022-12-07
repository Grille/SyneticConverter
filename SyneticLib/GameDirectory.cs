using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace SyneticLib;

public class GameDirectory : Ressource
{
    public RessourceDirectory<ScenarioVGroup> Scenarios;
    public RessourceDirectory<Car> Cars;
    public RessourceDirectory<Sound> Sounds;

    public GameDirectory(string path, GameVersion target = GameVersion.Auto) : base(null, path, PointerType.Directory)
    {
        if (target == GameVersion.Auto)
            Version = FindDirectoryGameVersion(path);
        else
            Version = target;

        Scenarios = new(this, ChildPath("Scenarios"),
            (path) => Directory.Exists(path),
            (path) => new ScenarioVGroup(this, path)
        );

        Cars = new(this, ChildPath("Autos"),
            (path) => Directory.Exists(path),
            (path) => new Car(this, path)
        );

        Sounds = new(this, ChildPath("Sounds"),
            (path) => File.Exists(path),
            (path) => new Sound(this, path)
        );
    }

    internal static GameDirectory Global = new("Global", GameVersion.WR2);

    public ScenarioVGroup GetScenario(string name)
    {
        var path = Path.Combine(SourcePath, "Scenarios", name);
        return new ScenarioVGroup(this, path);
    }

    public ScenarioVGroup CreateScenarioGroup(string name)
    {
        var path = Path.Combine(SourcePath, "Scenarios", name);



        return new ScenarioVGroup(this, path);
    }

    public static GameVersion FindDirectoryGameVersion(string path)
    {
        if (!Directory.Exists(path))
            return GameVersion.Invalid;

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
                "mbtr_pc.exe" => GameVersion.MBTR,
                "mbwr_pc.exe" => GameVersion.MBWR,
                "wr2_pc.exe" => GameVersion.WR2,
                "c11_pc.exe" => GameVersion.C11,
                "crashtime.exe" => GameVersion.CT1,
                "burningwheels.exe" => GameVersion.CT2,
                "ferrarivr.exe" => GameVersion.FVR,
                "highwaynights.exe" => GameVersion.CT3,
                "crashtime4.exe" => GameVersion.CT4,
                "crashtime5.exe" => GameVersion.CT5,

                "bn_setup.exe" => GameVersion.NICE2,
                "tr_setup.exe" => GameVersion.MBTR,
                "wr_setup.exe" => GameVersion.MBWR,
                "wr2_setup.exe" => GameVersion.WR2,
                "c11_setup.exe" => GameVersion.C11,
                "ct_setup.exe" => GameVersion.CT1,
                "bw_setup.exe" => GameVersion.CT2,
                "fvr_setup.exe" => GameVersion.FVR,
                "hn_setup.exe" => GameVersion.CT3,
                "ct4_setup.exe" => GameVersion.CT4,
                "ct5_config.exe" => GameVersion.CT5,

                _ => GameVersion.Invalid,
            };

            if (result != GameVersion.Invalid)
                return result;
        }

        return GameVersion.Invalid;
    }

    public static GameVersion ParseGameVersion(string version) => version switch
    {
        "NICE" => GameVersion.NICE,
        "NICE2" => GameVersion.NICE2,
        "MBWR" => GameVersion.MBWR,
        "WR2" => GameVersion.WR2,
        "C11" => GameVersion.C11,
        "CT1" => GameVersion.CT1,
        "CT2" => GameVersion.CT2,
        "FVR" => GameVersion.FVR,
        "CT3" => GameVersion.CT3,
        "CT4" => GameVersion.CT4,
        "CT5" => GameVersion.CT5,

        _ => GameVersion.Invalid,
    };

    protected override void OnLoad()
    {
        throw new NotImplementedException();
    }

    protected override void OnSave()
    {
        throw new NotImplementedException();
    }

    protected override void OnSeek()
    {
        Scenarios.TrySeek();
        Cars.TrySeek();
        Sounds.TrySeek();
    }
}
