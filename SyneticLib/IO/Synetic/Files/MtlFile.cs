using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace SyneticLib.IO.Synetic.Files;
public class MtlFile : Dictionary<int, MtlSection>
{
    private MtlSection UsedSection { get; set; }

    public string Path;

    public MtlFile()
    {
        Add(-1, new MtlSection());
        UsedSection = this[-1];
    }

    public void Load(string path = null)
    {
        if (path == null)
            path = Path;
         
        UsedSection = this[-1];

        var lines = File.ReadAllLines(path);
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            if (line.Length == 0)
                continue;

            if (line[0] == '#')
            {
                var split = line.Split(' ');
                var element = split[split.Length == 2 ? 1 : 3].Split('x', 2)[1];
                int id = int.Parse(element, NumberStyles.HexNumber);
                if (!ContainsKey(id))
                    Add(id, new());
                UsedSection = this[id];
            }
            else
            {
                var split = line.Split(' ', 2);
                var key = split[0];
                var value = split[1];
                Console.WriteLine($"{key}={value}");
                UsedSection.Add(key, value);
            }
        }
    }

    public void Save(string path)
    {

    }
}

public class MtlSection : Dictionary<string, string>
{
    public int GetIntFromHexArray(string key, int index)
    {
        var values = this[key].Split(' ');
        return int.Parse(values[index], NumberStyles.HexNumber);
    }
}
