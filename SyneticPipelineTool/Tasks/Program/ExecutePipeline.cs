using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool.Tasks.Program;

[PipelineTask(Key = "Program/Call pipeline")]
internal class ExecutePipeline : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Name");
    }

    protected override void OnExecute()
    {
        string name = EvalParameter("Name");

        var pipeline = Pipeline.Owner[name];

        Console.WriteLine($"Call {EvalParameter("Name")}");

        Runtime.Call(pipeline);

        Console.WriteLine($"Return");
    }

    public override Token[] ToTokens() => new Token[]
    {
        new(TokenType.Text, "Call "),
        new(TokenType.Variable, Parameters["Name"]),
    };
}
