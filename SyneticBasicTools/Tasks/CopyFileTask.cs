using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticPipelineTool.Tasks;

internal class CopyFileTask : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Src", "", "SrcFile");
        Parameters.Def(ParameterTypes.String, "Dst", "", "DstFile");
    }

    protected override void OnExecute()
    {
        string srcPath = EvalParameter("Src");
        string dstPath = EvalParameter("Dst");

        Console.WriteLine($"Copy dir {srcPath} to {dstPath}");

        File.Copy(srcPath, dstPath, true);
    }

    public override string ToString()
    {
        return $"Copy file {Parameters["Src"]} to {Parameters["Dst"]}";
    }
}
