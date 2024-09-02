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

[PipelineTask("Synetic/IO/Texture.Convert")]
internal class ConvertTexture : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.OpenFile, "Src File", "Load texture from file", "./texture.ptx");
        Parameters.Def(ParameterTypes.Enum, "Src Type", null, TextureTaskUtils.Default, TextureTaskUtils.FileTypes);
        Parameters.Def(ParameterTypes.SaveFile, "Dst File", "Save texture to file", "./texture.ptx");
        Parameters.Def(ParameterTypes.Enum, "Dst Type", null, TextureTaskUtils.Default, TextureTaskUtils.FileTypes);
    }

    protected override void OnExecute()
    {
        var srcPath = EvalParameter("Src File");
        var srcType = EvalParameter("Src Type");
        var dstPath = EvalParameter("Dst File");
        var dstType = EvalParameter("Dst Type");

        var texture = TextureTaskUtils.Load(srcPath, srcType);
        TextureTaskUtils.Save(dstPath, dstType, texture);
    }

    public override Token[] ToTokens() => new Token[]
    {
        new(TokenType.Text, "Texture.Save '"),
        new(TokenType.Expression, Parameters["Src File"]),
        new(TokenType.Text, "'("),
        new(TokenType.Expression, Parameters["Src Type"]),
        new(TokenType.Text, ") to "),
        new(TokenType.Expression, Parameters["Dst File"]),
        new(TokenType.Text, "'("),
        new(TokenType.Expression, Parameters["Dst Type"]),
        new(TokenType.Text, ")"),
    };
}
