using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib;

public class GameFolder : Ressource
{
    public RessourceDirectory<Scenario> Scenarios;
    public RessourceDirectory<Car> Cars;
    public RessourceDirectory<Sound> Sounds;

    public GameFolder(string path, GameVersion target = GameVersion.Auto) : base(null, PointerType.Directory)
    {
        SourcePath = path;

        if (target == GameVersion.Auto)
            Version = GetGameVersion(path);
        else
            Version = target;

        if (Version == GameVersion.Invalid)
            throw new ArgumentException(Version.ToString());

        Scenarios = new(this, ChildPath("Scenarios"),
            (path) => Directory.Exists(path),
            (path) => new Scenario(this, path)
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

    public Scenario GetScenario(string name)
    {
        var path = Path.Combine(SourcePath, "Scenarios", name);
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
                "crashtime.exe" => GameVersion.CTP,
                "burningwheels.exe" => GameVersion.CT2,
                "ferrarivr.exe" => GameVersion.FVR,
                "highwaynights.exe" => GameVersion.CT3,
                "crashtime4.exe" => GameVersion.CT4,
                "crashtime5.exe" => GameVersion.CT5,

                "bn_setup.exe" => GameVersion.NICE2,
                "wr_setup.exe" => GameVersion.MBWR,
                "wr2_setup.exe" => GameVersion.WR2,
                "c11_setup.exe" => GameVersion.C11,
                "ct_setup.exe" => GameVersion.CTP,
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
        Scenarios.Seek();
        Cars.Seek();
        Sounds.Seek();
    }
}
