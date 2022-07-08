using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib.IO.Synetic.Files;
public class MtlFile
{
    public Dictionary<string, Dictionary<string, string>> Data { get; set; } = new();
    public Dictionary<string, string> UsedSection { get; set; }

    public MtlFile()
    {
        Data.Add("_", new Dictionary<string, string>());
        UsedSection = Data["_"];
    }

    public void Load(string path)
    {
        UsedSection = Data["_"];
        var lines = File.ReadAllLines(path);
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            if (line.Length == 0)
                continue;

            if (line[0] == '#')
            {
                var split = line.Split(' ', 2);
                var section = split[1];
                Data.Add(section, new Dictionary<string, string>());
                UsedSection = Data[section];
            }
            else
            {
                var split = line.Split(' ', 2);
                var key = split[0];
                var value = split[1];
                UsedSection.Add(key, value);
            }
        }
    }

    public void Save(string path)
    {

    }


}
