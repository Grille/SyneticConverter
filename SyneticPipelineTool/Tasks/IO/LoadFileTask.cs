using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticPipelineTool.Tasks;

[PipelineTask(Name = "Load file")]
internal class LoadFileTask : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Src", "", "SrcFile");
        Parameters.Def(ParameterTypes.String, "Variable", "", "Var");
    }

    protected override void OnExecute()
    {
        var src = EvalParameter("Src");
        var var = EvalParameter("Variable");
        Runtime.Variables[var] = File.ReadAllText(src);
    }

    public override Token[] ToTokens() => new Token[]
    {
        new(TokenType.Text, "Load file "),
        new(TokenType.Variable, Parameters["Src"]),
        new(TokenType.Text, " as "),
        new(TokenType.Variable, Parameters["Variable"]),
    };
}
