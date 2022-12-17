using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticBasicTools.Tasks;

internal class ConvertScnFilesTask : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParamType.Path, "Path", "Path to directory containing scenario <Alps> or single variant <Alps/V1>.");
        Parameters.Def(ParamType.Version, "SrcVersion", "Allowed values [WR1]", "WR1");
        Parameters.Def(ParamType.Version, "DstVersion", "Allowed values [WR2]", "WR2");
    }

    protected override void OnExecute()
    {
        string path = GetValue("Path");
        WR1ToWR2Conv.ConvertVGroup(path);
        Console.WriteLine($"Convert files {GetValue("SrcVersion")} to {GetValue("DstVersion")} {path}");
    }

    public override string ToString()
    {
        return $"Convert files {Parameters["SrcVersion"]} to {Parameters["DstVersion"]} {Parameters["Path"]}";
    }
}
