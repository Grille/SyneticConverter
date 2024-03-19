using GGL.IO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool.Tasks.Program;

[PipelineTask("Program/Push")]
internal class Push : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Value", "Value pushed onto global stack.", "Var");
    }

    protected override void OnExecute()
    {
        var value = EvalParameter("Value");

        Runtime.ValueStack.Push(value);
    }

    public override Token[] ToTokens() => new Token[]
    {
        new Token(TokenType.Text, "Push "),
        new Token(TokenType.Variable, Parameters["Value"]),
    };
}
