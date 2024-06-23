using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

using SyneticLib.IO;

namespace SyneticLib.Locations;

public class GameDirectory : DirectoryLocation
{
    public LazyRessourceDirectory<ScenarioGroup> Scenarios { get; private set; }

    public LazyRessourceDirectory<Car> Cars { get; private set; }

    public LazyRessourceDirectory<Sound> Sounds { get; private set; }

    public GameVersion Version { get; set; }

    public GameDirectory(string path, GameVersion version) : base(path)
    {
        Version = version;
        Seek();
    }

    protected override void OnSeek()
    {
        Scenarios = new(ChildPath("Scenarios"),
            (path) => Directory.Exists(path),
            GetScenario
        );

        Cars = new(ChildPath("Autos"),
            (path) => Directory.Exists(path),
            (path) => null
        );

        Sounds = new(ChildPath("Sounds"),
            (path) => File.Exists(path),
            (path) => null
        );

        if (!Directory.Exists(DirectoryPath))
            return;

        Scenarios.Seek();
        Cars.Seek();
        Sounds.Seek();
    }

    public GameDirectory(string path) :
        this(path, GetDirectoryGameVersion(path))
    { }

    internal static GameDirectory Global = new("Global", GameVersion.WR2);

    public DirectoryLocation[] GetScenarios()
    {
        var dir = Path.Combine(DirectoryPath, "Scenarios");

        if (!Directory.Exists(dir))
            return Array.Empty<DirectoryLocation>();

        var paths = Directory.GetDirectories(dir);
        var result = new DirectoryLocation[paths.Length];
        for (var i = 0; i < paths.Length; i++) {
            result[i] = new DirectoryLocation(paths[i]);
        }
        return result;
    }

    public ScenarioGroup GetScenario(string name)
    {
        var path = Path.Combine(DirectoryPath, "Scenarios", name);
        var n = Path.GetFileName(path);
        return Serializers.ScenarioGroup.Synetic.Load(path, n);
    }

    public static GameVersion GetDirectoryGameVersion(string path)
    {
        if (!Directory.Exists(path))
            return GameVersion.None;

        var files = Directory.GetFiles(path);
        var names = new List<string>();

        foreach (var file in files)
            names.Add(Path.GetFileName(file).ToLower());

        foreach (var name in names)
        {
            var result = name switch
            {
                "nice1.exe" => GameVersion.NICE1,
                "breakneck.exe" => GameVersion.NICE2,
                "mbtr_pc.exe" => GameVersion.MBTR,
                "mbwr_pc.exe" => GameVersion.WR1,
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
                "wr_setup.exe" => GameVersion.WR1,
                "wr2_setup.exe" => GameVersion.WR2,
                "c11_setup.exe" => GameVersion.C11,
                "ct_setup.exe" => GameVersion.CT1,
                "bw_setup.exe" => GameVersion.CT2,
                "fvr_setup.exe" => GameVersion.FVR,
                "hn_setup.exe" => GameVersion.CT3,
                "ct4_setup.exe" => GameVersion.CT4,
                "ct5_config.exe" => GameVersion.CT5,

                _ => GameVersion.None,
            };

            if (result != GameVersion.None)
                return result;
        }

        return GameVersion.None;
    }

    public static GameVersion ParseGameVersion(string version) => version switch
    {
        "NICE1" => GameVersion.NICE1,
        "NICE2" => GameVersion.NICE2,
        "WR1" => GameVersion.WR1,
        "WR2" => GameVersion.WR2,
        "C11" => GameVersion.C11,
        "CT1" => GameVersion.CT1,
        "CT2" => GameVersion.CT2,
        "FVR" => GameVersion.FVR,
        "CT3" => GameVersion.CT3,
        "CT4" => GameVersion.CT4,
        "CT5" => GameVersion.CT5,

        "WR2CE" => GameVersion.WR2CE,

        _ => GameVersion.None,
    };
}
