using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticPipelineTool.Tasks.IO.System;

[PipelineTask(Key = "IO/Sys/Rename files in directory")]
internal class RenameFiles : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Dir", "", "Dir");
        Parameters.Def(ParameterTypes.String, "Old text", "", "Alpen");
        Parameters.Def(ParameterTypes.String, "New text", "", "MBWR_Alpen");
    }

    protected override void OnExecute()
    {
        string path = EvalParameter("Dir");
        string find = EvalParameter("Old text");
        string replace = EvalParameter("New text");

        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException(path);

        foreach (var file in Directory.GetFiles(path))
        {
            var dir = Path.GetDirectoryName(file);
            var newfile = Path.GetFileName(file).Replace(find, replace, StringComparison.CurrentCultureIgnoreCase);
            var newpath = Path.Combine(dir, newfile);
            File.Move(file, newpath);
        }
    }

    public override Token[] ToTokens() => new Token[]
    {
        new(TokenType.Text, "Rename files "),
        new(TokenType.Variable, Parameters["Old text"]),
        new(TokenType.Text, " -> "),
        new(TokenType.Variable, Parameters["New text"]),
        new(TokenType.Text, " in "),
        new(TokenType.Variable, Parameters["Dir"]),
    };
}
