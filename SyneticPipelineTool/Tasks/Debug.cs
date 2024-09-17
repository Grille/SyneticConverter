using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.PipelineTool;

using SyneticLib.Files;

namespace SyneticPipelineTool.Tasks;

[PipelineTask("Synetic/DEBUG")]
public class Debug : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.Generic, "Arg0", null, null);
    }

    protected override void OnExecute()
    {
        var arg0 = EvalParameter("Arg0");

        var file = new TrkFile();
        file.Load(arg0);
    }
}
