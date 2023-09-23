using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SyneticPipelineTool.Converter;

namespace SyneticPipelineTool.Tasks.Convert;

[PipelineTask(Key = "Converter/Convert MBWR sprites to WR2")]
internal class FixTreeNorm : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Dir", "", "Dir");
        Parameters.Def(ParameterTypes.String, "Tree List", "", "Tree0; Tree1;");
        Parameters.Def(ParameterTypes.Boolean, "Ignore Missing", "Skips missing files if they can't be found, elsewise throws error.", "false");
        Parameters.Def(ParameterTypes.String, "Diffuse Color", "", "0xa0a0a0");
        Parameters.Def(ParameterTypes.String, "Ambient Color", "", "0x202020");
    }

    protected override void OnExecute()
    {
        string dirPath = EvalParameter("Dir");
        var files = Parameter.ValueToList(EvalParameter("Tree List"));
        bool ignoreMissing = bool.Parse(EvalParameter("Ignore Missing"));
        string diffuse = EvalParameter("Diffuse Color");
        string ambient = EvalParameter("Ambient Color");

        foreach (string file in files)
        {
            string path = Path.Combine(dirPath, file);
            WR1ToWR2Conv.FixTreeSprite(path, ignoreMissing, diffuse, ambient);
        }
    }

    public override Token[] ToTokens() => new Token[]
    {
        new(TokenType.Text, "Fix Sprites "),
        new(TokenType.Variable, Parameters["Tree List"]),
        new(TokenType.Text, " in "),
        new(TokenType.Variable, Parameters["Dir"]),
    };
}

