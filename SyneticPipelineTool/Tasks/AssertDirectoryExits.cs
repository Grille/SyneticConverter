using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.PipelineTool;

namespace SyneticPipelineTool.Tasks;

[PipelineTask("Converter/AssertDirectoryExits")]
internal class AssertDirectoryExits : PipelineTask
{
    protected override void OnExecute()
    {

        var path = EvalParameter("DirPath");

        if (!Directory.Exists(path))
        {
            Runtime.Return();
        }
    }

    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "DirPath", "", "Dir");
    }
}
