using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.PipelineTool;
using Grille.PipelineTool.IO;
using SyneticLib.Utils;

namespace SyneticPipelineTool.Tasks;

[PipelineTask("Synetic/Convert Car Files")]
internal class ConvertCarFiles : PipelineTask
{
    static readonly Dictionary<string, Action<string, string>> _converters;

    static readonly string[] _keys;

    static ConvertCarFiles()
    {
        _converters = new Dictionary<string, Action<string, string>>
        {
            { "WR1 -> WR2", ConvertMbwrCar.Convert },
        };

        _keys = _converters.Keys.ToArray();
    }

    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.Directory, "Path", "Path to directory containing car");
        Parameters.Def(ParameterTypes.String, "Name", "car name e.g. <gmod>");
        Parameters.Def(ParameterTypes.Enum, "Converter", "", _keys[0], _keys);
    }

    protected override void OnExecute()
    {
        string key = EvalParameter("Converter");

        string path = EvalParameter("Path");
        string name = EvalParameter("Name");

        _converters[key](path, name);

        Runtime.Logger.Info($"Convert files {key} {path}");
    }

    public override Token[] ToTokens() => new Token[]
    {
        new Token(TokenType.Text, "Convert files from "),
        new Token(TokenType.Expression, Parameters["Converter"]),
        new Token(TokenType.Text, " "),
        new Token(TokenType.Expression, Parameters["Path"]),
    };
}
