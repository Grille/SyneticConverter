using GGL.IO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool.Tasks.Program;

[PipelineTask("Program/Assert")]
internal class Assert : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Name", "", "Var");
    }

    protected override void OnExecute()
    {
        var name = EvalParameter("Name");

        if (!Runtime.Variables.ContainsKey(name))
            throw new Exception();
    }

    public override Token[] ToTokens() => new Token[]
    {
        new Token(TokenType.Text, "In "),
        new Token(TokenType.Variable, Parameters["Name"]),
    };
}
