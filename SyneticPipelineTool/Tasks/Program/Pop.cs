using GGL.IO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool.Tasks.Program;

[PipelineTask("Program/Pop")]
internal class Pop : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Variable", "Variable to write the top value of the global stack into.", "Var");
    }

    protected override void OnExecute()
    {
        var name = EvalParameter("Variable");

        Runtime.Variables[name] = Runtime.ValueStack.Pop();
    }

    public override Token[] ToTokens() => new Token[]
    {
        new Token(TokenType.Text, "Pop "),
        new Token(TokenType.Variable, Parameters["Variable"]),
    };
}
