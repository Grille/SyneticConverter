using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticPipelineTool.Tasks;

internal class NopTask : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Text", "", "");
    }

    protected override void OnExecute()
    {
    }

    public override Token[] ToTokens()
    {
        var value = Parameters["Text"];
        var token = value == "" ? new Token(TokenType.Comment, "") : new Token(TokenType.Comment, $"// {value}");
        return new Token[] { token };
    }
}
