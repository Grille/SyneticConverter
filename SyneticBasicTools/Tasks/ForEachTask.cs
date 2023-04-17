using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticPipelineTool.Tasks;

internal class ForEachTask : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.Enum, "Mode", "", "List", new string[] { "List", "Directorys", "Files" });
        Parameters.Def(ParameterTypes.String, "Collection", "", "1");
        Parameters.Def(ParameterTypes.String, "Variable", "", "i");
    }

    protected override void OnExecute()
    {
        string mode = EvalParameter("Mode");
        string collection = EvalParameter("Collection");
        string variable = EvalParameter("Variable");

        var next = Pipeline.Tasks[Pipeline.TaskPosition += 1];
        if (next == null)
            throw new NullReferenceException();

        string[] items = mode switch
        {
            "List" => collection.Split(',', StringSplitOptions.RemoveEmptyEntries),
            "Directorys" => Directory.GetDirectories(collection),
            "Files" => Directory.GetDirectories(collection),
            _ => throw new ArgumentOutOfRangeException(mode)
        };

        foreach (var item in items)
        {
            Pipeline.Variables[variable] = item.Trim();
            next.Execute();
        }

    }

    public override string ToString()
    {
        return $"Foreach {Parameters["Variable"]} in {Parameters["Collection"]}:";
    }
}
