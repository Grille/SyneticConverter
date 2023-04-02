using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticPipelineTool.Tasks;

internal class LoadFileTask : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Src", "", "SrcFile");
        Parameters.Def(ParameterTypes.String, "Variable", "", "Var");
    }

    protected override void OnExecute()
    {
        var src = GetValue("Src");
        var var = GetValue("Variable");
        Pipeline.Variables[var] = File.ReadAllText(src);
    }

    public override string ToString()
    {
        return $"Load file {Parameters["Src"]} as {Parameters["Variable"]}";
    }
}
