using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;

namespace SyneticPipelineTool.Tasks;

[PipelineTask(Name = "For Each")]
internal class ForEachTask : PipelineTask
{
    /*
    public Parameter Mode { get; } = new ParameterEnum("Mode", "", "list", new string[] { "List", "Directorys", "Files" });
    public Parameter Collection { get; } = new ParameterString("Collection", "", "1");
    public Parameter Variable { get; } = new ParameterString("Variable", "", "i");
    */

    protected override void OnInit()
    {
        Parameters.Add(
            new ParameterEnum("Mode", "", "List", new string[] { "List", "Directorys", "Files" }),
            new ParameterString("Collection", "", "1"),
            new ParameterString("Variable", "", "i")
        );
    }

    protected override void OnExecute()
    {
        string mode = EvalParameter("Mode");
        string collection = EvalParameter("Collection");
        string variable = EvalParameter("Variable");

        var next = Pipeline.Tasks[Runtime.Position += 1];
        if (next == null)
            throw new NullReferenceException();

        string[] items = mode switch
        {
            "List" => Parameter.ValueToList(collection).ToArray(),
            "Directorys" => Directory.GetDirectories(collection),
            "Files" => Directory.GetFiles(collection),
            _ => throw new ArgumentOutOfRangeException(mode)
        };

        foreach (var item in items)
        {
            Runtime.Variables[variable] = item.Trim();
            next.Execute(Runtime);
        }

    }

    public override Token[] ToTokens() => new Token[]
    {
        new Token(TokenType.Text, $"Foreach "),
        new Token(TokenType.Variable, Parameters["Variable"]),
        new Token(TokenType.Text, $" in "),
        new Token(TokenType.Variable, Parameters["Collection"]),
        new Token(TokenType.Text, $":"),
    };
}
