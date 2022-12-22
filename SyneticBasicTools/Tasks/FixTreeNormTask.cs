using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticBasicTools.Tasks;

internal class FixTreeNormTask : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParamType.Path, "Dir", "", "Dir");
        Parameters.Def(ParamType.Path, "Tree List", "", "Tree0; Tree1;");
    }

    protected override void OnExecute()
    {
        string dirPath = GetValue("Dir");
        string filesStr = GetValue("Tree List");

        var files = new List<string>();

        foreach (string file in filesStr.Split(';', StringSplitOptions.RemoveEmptyEntries))
        {
            string trim = file.Trim();
            if (trim.Length > 0)
                files.Add(trim);
        }

        foreach (string file in files)
        {
            string path = Path.Combine(dirPath, file);
            WR1ToWR2Conv.FixTreeSprite(path);
        }


        Console.WriteLine($"Fix Sprites {filesStr}");
    }

    public override string ToString()
    {
        return $"Fix Sprites {Parameters["Tree List"]} in {Parameters["Dir"]}";
    }
}
