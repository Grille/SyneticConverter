using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticConverter;

public  class GameFolder
{
    public string RootDir;
    public TargetFormat Target;


    public GameFolder(string path, TargetFormat target)
    {
        RootDir = path;
        Target = target;
    }

    public Dictionary<string, Scenario> GetAllScenarios()
    {
        var scnRootDir = Directory.GetDirectories(Path.Combine(RootDir, "Scenarios"));

        var dict = new Dictionary<string, Scenario>();
        foreach (var path in scnRootDir)
        {
            var name = Path.GetFileName(path);
            dict.Add(name, GetScenario(name));
        }

        return dict;
    }

    public Scenario GetScenario(string name)
    {
        var path = Path.Combine(RootDir, "Scenarios", name);
        return new Scenario(this, path);
    }

}
