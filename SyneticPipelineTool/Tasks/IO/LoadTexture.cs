﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

using Grille.PipelineTool;
using Grille.PipelineTool.IO;

using SyneticLib.IO;

namespace SyneticPipelineTool.Tasks.IO;

[PipelineTask("Synetic/IO/Texture.Load")]
internal class LoadTexture : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.OpenFile, "Src File", "Load texture from file", "./texture.ptx");
        Parameters.Def(ParameterTypes.Enum, "File Type", null, SerializerTaskUtils.Default, SerializerTaskUtils.TextureFileTypes);
        Parameters.DefResult("Texture");
    }

    protected override void OnExecute()
    {
        var path = EvalParameter("Src File");
        var type = EvalParameter("File Type");
        var texturename = EvalParameter("Texture");

        Runtime.Variables[texturename] = SerializerTaskUtils.LoadTexture(path, type);
    }

    public override Token[] ToTokens() => new Token[]
    {
        new(TokenType.Text, "Texture.Load "),
        new(TokenType.Expression, Parameters["Src File"]),
        new(TokenType.Text, " as "),
        new(TokenType.Expression, Parameters["Texture"]),
    };
}
