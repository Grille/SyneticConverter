using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.PipelineTool;
using Grille.PipelineTool.IO;
using SyneticLib.Utils;
using SyneticLib;

namespace SyneticPipelineTool.Tasks.IO;

[PipelineTask("Synetic/IO/Load RandObj As Texture")]
internal class LoadRandObjAsTexture : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.OpenFile, "RandObj", "RandObj File");
        Parameters.DefResult();
    }

    protected override void OnExecute()
    {
        string file = EvalParameter("RandObj");
        string path = EvalParameter("Result");

        int size = 1024;

        var bgra = RandObjToBgraConverter.Convert(file, size);

        var level = new TextureLevel(size, size, bgra);
        var texture = new Texture(TextureFormat.Bgra32, level);

        Runtime.Variables[path] = new VariableValue(texture);
    }
}
