using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool.Tasks;

internal class ExecutePipelineTask : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Name");
    }
    protected override void OnExecute()
    {
        string name = EvalParameter("Name");

        var pipeline = Pipeline.Owner[name];
        foreach (var key in Pipeline.Variables.Keys)
            pipeline.Variables[key] = Pipeline.Variables[key];
        Console.WriteLine($"Call {EvalParameter("Name")}");
        pipeline.Execute(Pipeline.CallStack);
        Console.WriteLine($"Return");
    }

    public override string ToString()
    {
        return $"Call {Parameters["Name"]}";
    }
}
