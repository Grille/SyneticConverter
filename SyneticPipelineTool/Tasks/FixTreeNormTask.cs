using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticPipelineTool.Tasks;

internal class FixTreeNormTask : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Dir", "", "Dir");
        Parameters.Def(ParameterTypes.String, "Tree List", "", "Tree0; Tree1;");
        Parameters.Def(ParameterTypes.Boolean, "Ignore Missing", "Skips missing files if they can't be found, elsewise throws error.", "false");
        Parameters.Def(ParameterTypes.String, "Diffuse Color", "", "0xa0a0a0");
        Parameters.Def(ParameterTypes.String, "Ambient Color", "", "0x202020");
    }

    protected override void OnExecute()
    {
        string dirPath = EvalParameter("Dir");
        var files = Parameter.ValueToList(EvalParameter("Tree List"));
        bool ignoreMissing = bool.Parse(EvalParameter("Ignore Missing"));
        string diffuse = EvalParameter("Diffuse Color"); 
        string ambient = EvalParameter("Ambient Color");

        foreach (string file in files)
        {
            string path = Path.Combine(dirPath, file);
            WR1ToWR2Conv.FixTreeSprite(path, ignoreMissing, diffuse, ambient);
        }
    }

    public override string ToString()
    {
        return $"Fix Sprites {Parameters["Tree List"]} in {Parameters["Dir"]}";
    }
}
