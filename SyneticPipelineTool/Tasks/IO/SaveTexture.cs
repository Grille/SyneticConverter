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

[PipelineTask("Synetic/IO/Texture.Save")]
internal class SaveTexture : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.SaveFile, "Dst File", "Save texture to file", "./texture.ptx");
        Parameters.Def(ParameterTypes.Enum, "File Type", null, TextureTaskUtils.Default, TextureTaskUtils.FileTypes);
        Parameters.Def(ParameterTypes.Object, "Texture", null, "*Texture", null);
    }

    protected override void OnExecute()
    {
        var path = EvalParameter("Dst File");
        var type = EvalParameter("File Type");
        var texture = EvalParameter("Texture");

        TextureTaskUtils.Save(path, type, texture);
    }

    public override Token[] ToTokens() => new Token[]
    {
        new(TokenType.Text, "Texture.Save "),
        new(TokenType.Expression, Parameters["Texture"]),
        new(TokenType.Text, " in "),
        new(TokenType.Expression, Parameters["Dst File"]),
    };
}
