using GGL.IO;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticPipelineTool.Converter;

namespace SyneticPipelineTool.Tasks;

[PipelineTask(Name = "Convert scenario files")]
internal class ConvertScnFilesTask : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Path", "Path to directory containing scenario variant e.g. <C:/Alps/V1>.");
        Parameters.Def(ParameterTypes.String, "Name", "Scenario name e.g. <Alps>");
        Parameters.Def(ParameterTypes.Enum, "SrcVersion", "", "WR1", new[] { "WR1", "C11" });
        Parameters.Def(ParameterTypes.Enum, "DstVersion", "", "WR2", new[] { "WR2" });
        Parameters.Def(ParameterTypes.Single, "Ambient Light", "", "0.22");
    }

    protected override void OnExecute()
    {
        string verSrc = EvalParameter("SrcVersion");
        string verDst = EvalParameter("DstVersion");

        string path = EvalParameter("Path");
        string name = EvalParameter("Name");
        float ambient = float.Parse(EvalParameter("Ambient Light"));

        if (verSrc == "WR1")
        WR1ToWR2Conv.Convert(path, ambient);
        if (verSrc == "C11")
            C11ToWR2Conv.Convert(path);

        Console.WriteLine($"Convert files {EvalParameter("SrcVersion")} to {EvalParameter("DstVersion")} {path}");
    }

    public override string ToString()
    {
        return $"Convert files from {Parameters["SrcVersion"]} to {Parameters["DstVersion"]} {Parameters["Path"]}";
    }
}
