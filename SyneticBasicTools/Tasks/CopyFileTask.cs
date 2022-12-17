using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticBasicTools.Tasks;

internal class CopyFileTask : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParamType.Path, "Src", "", "SrcFile");
        Parameters.Def(ParamType.Path, "Dst", "", "DstFile");
    }

    protected override void OnExecute()
    {
        string srcPath = GetValue("Src");
        string dstPath = GetValue("Dst");

        Console.WriteLine($"Copy dir {srcPath} to {dstPath}");

        File.Copy(srcPath, dstPath, true);
    }

    public override string ToString()
    {
        return $"Copy file {Parameters["Src"]} to {Parameters["Dst"]}";
    }
}
