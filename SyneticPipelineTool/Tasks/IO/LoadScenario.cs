using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

using Grille.PipelineTool;
using Grille.PipelineTool.IO;

using SyneticLib.IO;

namespace SyneticPipelineTool.Tasks.IO;

[PipelineTask("Synetic/IO/Scenario.Load")]
internal class LoadScenario : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.OpenFile, "Src File", "Load model", "./alpen/alpen.qad");
        Parameters.Def(ParameterTypes.Enum, "Type", null, SerializerTaskUtils.ScenarioTypes[0], SerializerTaskUtils.ScenarioTypes);
        Parameters.DefResult("Scenario");
    }

    protected override void OnExecute()
    {
        var path = EvalParameter("Src File");
        var type = EvalParameter("Type");
        var name = EvalParameter("Scenario");

        Runtime.Variables[name] = SerializerTaskUtils.LoadScenario(path, type);
    }

    public override Token[] ToTokens() => new Token[]
    {
        new(TokenType.Text, "Scenario.Load "),
        new(TokenType.Expression, Parameters["Src File"]),
        new(TokenType.Text, " as "),
        new(TokenType.Expression, Parameters["Scenario"]),
    };
}
