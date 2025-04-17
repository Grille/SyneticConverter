using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.PipelineTool;
using Grille.PipelineTool.IO;
using SyneticLib.Utils;
using SyneticLib;

namespace SyneticPipelineTool.Tasks;

[PipelineTask("Synetic/Convert scenario files")]
internal class ConvertScnFiles : PipelineTask
{
    delegate void Action3(string path, string name, Action<string> callback);
    delegate void Action4(string path, string name, Action<string> callback, GameVersion version);

    static readonly Dictionary<string, Action3> _converters;

    static readonly string[] _keys;

    static ConvertScnFiles()
    {
        static Action3 Action43(Action4 action, GameVersion version) => (a, b, c) => action(a, b, c, version);

        _converters = new Dictionary<string, Action3>
        {
            { "WR1 -> WR2", WR1ToWR2FileConv.Convert },
            { "C11 -> WR2", C11ToWR2FileConv.Convert },
            { "CT1 -> WR2", Action43(CT1ToWR2FileConv.Convert, GameVersion.CT1) },
            { "CT2 -> WR2", Action43(CT1ToWR2FileConv.Convert, GameVersion.CT2) },
            { "CT5 -> WR2", Action43(CT1ToWR2FileConv.Convert, GameVersion.CT5) }
        };

        _keys = _converters.Keys.ToArray();
    }

    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.Directory, "Path", "Path to directory containing scenario variant e.g. <C:/Alps/V1>.");
        Parameters.Def(ParameterTypes.String, "Name", "Scenario name e.g. <Alps>");
        Parameters.Def(ParameterTypes.Enum, "Converter", "", _keys[0], _keys);
    }

    protected override void OnExecute()
    {
        string key = EvalParameter("Converter");

        string path = EvalParameter("Path");
        string name = EvalParameter("Name");

        Runtime.Logger.Info($"Convert files {key} {path}");
        Runtime.Logger.IncScope();

        _converters[key](path, name, Runtime.Logger.Info);

        Runtime.Logger.DecScope();
    }

    public override Token[] ToTokens() => new Token[]
    {
        new Token(TokenType.Text, "Convert files from "),
        new Token(TokenType.Expression, Parameters["Converter"]),
        new Token(TokenType.Text, " "),
        new Token(TokenType.Expression, Parameters["Path"]),
    };
}
