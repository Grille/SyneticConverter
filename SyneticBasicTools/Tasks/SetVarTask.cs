using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticBasicTools.Tasks;

internal class SetVarTask : PipelineTask
{

    protected override void OnInit()
    {
        Parameters.Def("Name", "String", "", "Name");
        Parameters.Def("Value", "String", "", "Value");
    }

    protected override void OnExecute()
    {
        Pipeline.Variables[Parameters["Name"]] = Parameters["Value"];
    }

    public override string ToString()
    {
        return $"{Parameters["Name"]} = {Parameters["Value"]}";
    }
}
