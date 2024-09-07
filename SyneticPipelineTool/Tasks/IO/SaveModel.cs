using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.PipelineTool;
using Grille.PipelineTool.IO;

using SyneticLib;
using SyneticLib.IO;

namespace SyneticPipelineTool.Tasks.IO;

//[PipelineTask("Synetic/IO/Model.Save")]
internal class SaveModel : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.SaveFile, "Dst Path", "Save model", "./model.mox");
        Parameters.Def(ParameterTypes.Enum, "Type", null, SerializerTaskUtils.ModelTypes[0], SerializerTaskUtils.ModelTypes);
        Parameters.Def(ParameterTypes.Object, "Model", null, "*Model", null);
    }

    protected override void OnExecute()
    {
        var path = EvalParameter("Dst Path");
        var type = EvalParameter("Type");
        var name = EvalParameter("Model");

        SerializerTaskUtils.SaveModel(path, type, name);
    }

    public override Token[] ToTokens() => new Token[]
    {
        new(TokenType.Text, "Model.Save "),
        new(TokenType.Expression, Parameters["Model"]),
        new(TokenType.Text, " in "),
        new(TokenType.Expression, Parameters["Dst Path"]),
    };
}
