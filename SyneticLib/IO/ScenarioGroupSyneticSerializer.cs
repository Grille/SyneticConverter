using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.World;

namespace SyneticLib.IO;
public class ScenarioGroupSyneticSerializer : DirectorySerializer<ScenarioGroup>
{
    public ScenarioGroup Load(string path, string name)
    {
        var dirs = Directory.GetDirectories(path);

        var variants = new List<Scenario>();

        for (int i = 0; i < dirs.Length; i++)
        {
            var dirpath = dirs[i];
            string dirname = Path.GetFileName(dirpath);
            if (dirname.Length == 2 && int.TryParse(dirname.AsSpan(1, 1), out int id))
            {
                var scenario = Serializers.Scenario.Synetic.Load(dirpath, name);
                variants.Add(scenario);
            }
        }

        var group = new ScenarioGroup(name, variants.ToArray());
        return group;
    }

    protected override ScenarioGroup OnLoad(string dirPath)
    {
        throw new NotImplementedException();
    }

    protected override void OnSave(string dirPath, ScenarioGroup obj)
    {
        throw new NotImplementedException();
    }
}
