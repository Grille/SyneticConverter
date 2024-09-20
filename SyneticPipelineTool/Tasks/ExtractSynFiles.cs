using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.PipelineTool;

using SyneticLib.Files;

namespace SyneticPipelineTool.Tasks;

[PipelineTask("Synetic/Extract Syn Files")]
public class ExtractSynFiles : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.Directory, "Directory Path", null, null, null);
        Parameters.Def(ParameterTypes.Boolean, "Recursive", "Extract .syn files in subfolders recursively.", "false", null);
        Parameters.Def(ParameterTypes.Boolean, "Remove Syn Files", "Delete .syn files after extraction.", "false", null);

    }

    protected override void OnExecute()
    {
        var arg0 = EvalParameter("Directory Path");
        var arg1 = EvalParameter("Recursive") == "true";
        var arg2 = EvalParameter("Remove Syn Files") == "true";

        SynFile.ExtractFilesInDirectory(arg0, arg1, arg2);
    }
}
