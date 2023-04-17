using GGL.IO;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool.Tasks;

internal class ConvertScnFilesTask : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Path", "Path to directory containing scenario variant e.g. <C:/Alps/V1>.");
        Parameters.Def(ParameterTypes.String, "Name", "Scenario name e.g. <Alps>");
        Parameters.Def(ParameterTypes.Enum, "SrcVersion", "", "WR1", new[] { "WR1" });
        Parameters.Def(ParameterTypes.Enum, "DstVersion", "", "WR2", new[] { "WR2" });
        Parameters.Def(ParameterTypes.Single, "Ambient Light", "", "0.22");
    }

    protected override void OnExecute()
    {
        string path = EvalParameter("Path");
        string name = EvalParameter("Name");
        float ambient = float.Parse(EvalParameter("Ambient Light"));

        WR1ToWR2Conv.ConvertV(path, name, ambient);
        Console.WriteLine($"Convert files {EvalParameter("SrcVersion")} to {EvalParameter("DstVersion")} {path}");
    }

    public override string ToString()
    {
        return $"Convert files from {Parameters["SrcVersion"]} to {Parameters["DstVersion"]} {Parameters["Path"]}";
    }
}
