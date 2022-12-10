using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticBasicTools.Tasks;

internal class ExecutePipelineTask : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def("Name", "String");
    }
    protected override void OnExecute()
    {
        string name = Parameters["Name"];
        if (name == Pipeline.Name)
            throw new InvalidOperationException();

        //Pipeline.PiplineList.;
    }

    public override string ToString()
    {
        return $"Run Pipeline {Parameters["Name"]}";
    }
}
