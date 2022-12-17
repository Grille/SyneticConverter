using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticBasicTools.Tasks;

internal class RemoveDirTask : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParamType.Path, "Dir", "", "Dir");
    }

    protected override void OnExecute()
    {
        string path = GetValue("Dir");
        Directory.Delete(path, true);

        Console.WriteLine($"Remove dir {path}");
    }

    public override string ToString()
    {
        return $"Remove dir {Parameters["Dir"]}";
    }
}
