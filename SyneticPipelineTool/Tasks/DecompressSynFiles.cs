using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.PipelineTool;

using SyneticLib.Files;

namespace SyneticPipelineTool.Tasks;

[PipelineTask("Synetic/Decompress Syn Files")]
public class DecompressSynFiles : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.Directory, "Directory Path", null, null, null);
        Parameters.Def(ParameterTypes.Boolean, "Recursive", null, "false", null);
        Parameters.Def(ParameterTypes.Boolean, "Remove Syn Files", null, "false", null);

    }

    protected override void OnExecute()
    {
        var arg0 = EvalParameter("Directory Path");
        var arg1 = EvalParameter("Recursive") == "true";
        var arg2 = EvalParameter("Remove Syn Files") == "true";

        SynFile.DecompressDirectory(arg0, arg1, arg2);
    }
}
