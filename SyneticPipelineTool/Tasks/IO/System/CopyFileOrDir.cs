using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticPipelineTool.Tasks.IO.System;

[PipelineTask(Key = "IO/Sys/Copy file or directory")]
internal class CopyFileOrDir : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.Enum, "Mode", "", "File", new[] { "File", "Directory" });
        Parameters.Def(ParameterTypes.String, "Src", "", "SrcDir");
        Parameters.Def(ParameterTypes.String, "Dst", "", "DstFile");
    }

    protected override void OnExecute()
    {
        string mode = EvalParameter("Mode");
        string srcPath = EvalParameter("Src");
        string dstPath = EvalParameter("Dst");

        switch (mode)
        {
            case "File":
                File.Copy(srcPath, dstPath, true);
                break;

            case "Directory":

                if (!Directory.Exists(dstPath))
                    Directory.CreateDirectory(dstPath);

                //Now Create all of the directories
                foreach (string dirPath in Directory.GetDirectories(srcPath, "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(srcPath, dstPath));
                }

                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(srcPath, "*.*", SearchOption.AllDirectories))
                {
                    File.Copy(newPath, newPath.Replace(srcPath, dstPath), true);
                }

                Console.WriteLine($"Copy dir {srcPath} to {dstPath}");
                break;
        }
    }

    public override Token[] ToTokens() => new Token[]
    {
        new(TokenType.Text, "Copy "),
        new(TokenType.Variable, Parameters["Mode"]),
        new(TokenType.Text, " "),
        new(TokenType.Variable, Parameters["Src"]),
        new(TokenType.Text, " to "),
        new(TokenType.Variable, Parameters["Dst"]),
    };
}
