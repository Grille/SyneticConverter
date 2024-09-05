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

[PipelineTask("Synetic/IO/Mesh.Save")]
internal class SaveMesh : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.SaveFile, "Dst File", "Save mesh to file", "./mesh.mox");
        Parameters.Def(ParameterTypes.Enum, "File Type", null, SerializerTaskUtils.Default, SerializerTaskUtils.MeshFileTypes);
        Parameters.Def(ParameterTypes.Object, "Mesh", null, "*Mesh", null);
    }

    protected override void OnExecute()
    {
        var path = EvalParameter("Dst File");
        var type = EvalParameter("File Type");
        var texture = EvalParameter("Mesh");

        SerializerTaskUtils.SaveMesh(path, type, texture);
    }

    public override Token[] ToTokens() => new Token[]
    {
        new(TokenType.Text, "Mesh.Save "),
        new(TokenType.Expression, Parameters["Mesh"]),
        new(TokenType.Text, " in "),
        new(TokenType.Expression, Parameters["Dst File"]),
    };
}
