using GGL.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool.Tasks.Program;

[PipelineTask(Key = "Program/Run Process")]
internal class RunProcess : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Path");
        Parameters.Def(ParameterTypes.String, "Args");
    }

    protected override void OnExecute()
    {
        string path = EvalParameter("Path");
        string args = EvalParameter("Args");

        var p = new Process();
        p.StartInfo.FileName = path;
        p.StartInfo.Arguments = args;
        p.Start();
        p.WaitForExit();
    }

    public override Token[] ToTokens() => new Token[]
    {
        new(TokenType.Text, "Run "),
        new(TokenType.Variable, Parameters["Path"]),
        new(TokenType.Text, " "),
        new(TokenType.Variable, Parameters["Args"]),
    };
}
