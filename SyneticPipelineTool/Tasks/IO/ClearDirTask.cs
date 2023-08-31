using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticPipelineTool.Tasks;

[PipelineTask(Name = "Clear directory")]
internal class ClearDirTask : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Dir", "", "Dir");
    }

    protected override void OnExecute()
    {
        string path = EvalParameter("Dir");

        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException(path);

        foreach (var file in Directory.GetFiles(path))
            File.Delete(file);

        foreach (var dir in Directory.GetDirectories(path))
            Directory.Delete(dir, true);

        Console.WriteLine($"Clear dir {path}");
    }

    public override Token[] ToTokens() => new Token[]
    {
        new(TokenType.Text, $"Clear dir "),
        new(TokenType.Variable, Parameters["Dir"]),
    };
}
