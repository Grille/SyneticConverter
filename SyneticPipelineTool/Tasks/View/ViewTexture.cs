using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.PipelineTool;
using Grille.PipelineTool.IO;

using SyneticLib;
using SyneticLib.IO;

using SyneticPipelineTool.GUI;

namespace SyneticPipelineTool.Tasks.IO;

[PipelineTask("Synetic/Debug/Display Texture")]
internal class ViewTexture : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Texture", null, "*Texture", null);
    }

    protected override void OnExecute()
    {
        var texture = EvalParameter("Texture");

        TextureViewDialog.ShowDialog((Texture)texture.Value);
    }

    public override Token[] ToTokens() => new Token[]
    {
        new(TokenType.Text, "View Texture "),
        new(TokenType.Expression, Parameters["Texture"]),
    };
}
