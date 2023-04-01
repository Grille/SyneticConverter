using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticPipelineTool.Tasks;

internal class NopTask : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParamType.String, "Text", "", "");
    }

    protected override void OnExecute()
    {
    }

    public override string ToString()
    {
        return $"// {Parameters["Text"]}";
    }
}
