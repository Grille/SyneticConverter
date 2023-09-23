using GGL.IO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool.Tasks.Program;

//[PipelineTask(Key = "Program/Label")]
internal class Label : PipelineTask
{

    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Name", "", "Label");
    }

    protected override void OnExecute()
    {
    }

    public override Token[] ToTokens() => new Token[]
    {
        new Token(TokenType.Flow, Parameters["Name"] + ':')
    };
}
