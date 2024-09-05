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

[PipelineTask("Synetic/IO/Mesh.Load")]
internal class LoadMesh : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.OpenFile, "Src File", "Load mesh from file", "./mesh.mox");
        Parameters.Def(ParameterTypes.Enum, "File Type", null, SerializerTaskUtils.Default, SerializerTaskUtils.MeshFileTypes);
        Parameters.DefResult("Mesh");
    }

    protected override void OnExecute()
    {
        var path = EvalParameter("Src File");
        var type = EvalParameter("File Type");
        var texturename = EvalParameter("Mesh");

        Runtime.Variables[texturename] = SerializerTaskUtils.LoadMesh(path, type);
    }

    public override Token[] ToTokens() => new Token[]
    {
        new(TokenType.Text, "Mesh.Load "),
        new(TokenType.Expression, Parameters["Src File"]),
        new(TokenType.Text, " as "),
        new(TokenType.Expression, Parameters["Mesh"]),
    };
}
