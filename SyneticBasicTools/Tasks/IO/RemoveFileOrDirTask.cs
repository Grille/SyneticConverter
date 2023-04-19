using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticPipelineTool.Tasks;

internal class RemoveFileOrDirTask : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.Enum, "Mode", "", "File", new[] { "File", "Directory" });
        Parameters.Def(ParameterTypes.String, "Path", "", "Dir");
    }

    protected override void OnExecute()
    {
        string mode = EvalParameter("Mode");
        string path = EvalParameter("Path");

        switch (mode)
        {
            case "File":
                File.Delete(path);
                break;

            case "Directory":

                Directory.Delete(path, true);
                break;
        }

        Console.WriteLine($"Remove dir {path}");
    }

    public override string ToString()
    {
        return $"Remove {Parameters["Mode"]} {Parameters["Path"]}";
    }
}
