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
        Parameters.Def("Src", "String", "", "SrcFile");
        Parameters.Def("Dst", "String", "", "DstFile");
    }

    protected override void OnExecute()
    {
        string srcPath = Pipeline.PathFromVar(Parameters["Src"]);
        string dstPath = Pipeline.PathFromVar(Parameters["Dst"]);
        File.Copy(srcPath, dstPath, true);
    }

    public override string ToString()
    {
        return $"Copy File {Parameters["Src"]} To {Parameters["Dst"]}";
    }
}
