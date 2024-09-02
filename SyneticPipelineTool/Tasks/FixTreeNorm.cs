using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Grille.PipelineTool;
using Grille.PipelineTool.IO;
using SyneticLib.Conversion;

namespace SyneticPipelineTool.Tasks;

[PipelineTask("Synetic/Convert MBWR sprites to WR2")]
internal class FixTreeNorm : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.Directory, "Dir", "", "Dir");
        Parameters.Def(ParameterTypes.String, "Tree List", "", "Tree0; Tree1;");
        Parameters.Def(ParameterTypes.Boolean, "Ignore Missing", "Skips missing files if they can't be found, elsewise throws error.", "false");
        Parameters.Def(ParameterTypes.Color, "Diffuse Color", "", "ffa0a0a0");
        Parameters.Def(ParameterTypes.Color, "Ambient Color", "", "ff202020");
    }

    protected override void OnExecute()
    {
        string dirPath = EvalParameter("Dir");
        var files = EvalParameter("Tree List");
        bool ignoreMissing = bool.Parse(EvalParameter("Ignore Missing"));
        string diffuse = EvalParameter("Diffuse Color");
        string ambient = EvalParameter("Ambient Color");

        foreach (string file in files.GetEnumerator())
        {
            string path = Path.Combine(dirPath, file);
            WR1ToWR2FileConv.FixTreeSprite(path, ignoreMissing, diffuse, ambient);
        }
    }

    public override Token[] ToTokens() => new Token[]
    {
        new(TokenType.Text, "Fix Sprites "),
        new(TokenType.Expression, Parameters["Tree List"]),
        new(TokenType.Text, " in "),
        new(TokenType.Expression, Parameters["Dir"]),
    };
}

