using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticBasicTools.Tasks;

internal class CopyDirTask : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParamType.Path, "Src", "", "SrcDir");
        Parameters.Def(ParamType.Path, "Dst", "", "DstFile");
    }

    protected override void OnExecute()
    {
        string sourcePath = GetValue("Src");
        string targetPath = GetValue("Dst");

        if (!Directory.Exists(targetPath))
            Directory.CreateDirectory(targetPath);

        //Now Create all of the directories
        foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
        }

        //Copy all the files & Replaces any files with the same name
        foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
        {
            File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
        }

        Console.WriteLine($"Copy dir {sourcePath} to {targetPath}");
    }

    public override string ToString()
    {
        return $"Copy dir {Parameters["Src"]} to {Parameters["Dst"]}";
    }
}
