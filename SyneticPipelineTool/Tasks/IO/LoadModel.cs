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

[PipelineTask("Synetic/IO/Model.Load")]
internal class LoadModel : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.OpenFile, "Src Path", "Load model", "./model.mox");
        Parameters.Def(ParameterTypes.Enum, "Type", null, SerializerTaskUtils.ModelTypes[0], SerializerTaskUtils.ModelTypes);
        Parameters.DefResult("Model");
    }

    protected override void OnExecute()
    {
        var path = EvalParameter("Src Path");
        var type = EvalParameter("Type");
        var name = EvalParameter("Model");

        Runtime.Variables[name] = SerializerTaskUtils.LoadModel(path, type);
    }

    public override Token[] ToTokens() => new Token[]
    {
        new(TokenType.Text, "Model.Load "),
        new(TokenType.Expression, Parameters["Src Path"]),
        new(TokenType.Text, " as "),
        new(TokenType.Expression, Parameters["Model"]),
    };
}
