using GGL.IO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool.Tasks.Program;

[PipelineTask("Program/Output")]
internal class Output : PipelineTask
{

    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Name", "", "Var");
        Parameters.Def(ParameterTypes.String, "Value", "", "Value");
    }

    protected override void OnExecute()
    {
        var name = EvalParameter("Name");

        if (!Runtime.Variables.ContainsKey(name))
            throw new Exception();
    }

    public override Token[] ToTokens() => new Token[]
    {
        new Token(TokenType.Text, "Out "),
        new Token(TokenType.Variable, Parameters["Value"]),
        new Token(TokenType.Text, " as "),
        new Token(TokenType.Variable, Parameters["Name"]),
    };
}
