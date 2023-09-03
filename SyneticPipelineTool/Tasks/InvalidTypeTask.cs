using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticPipelineTool.Tasks;

internal class InvalidTypeTask : PipelineTask
{
    public string AssemblyQualifiedName { get; }
    public int ParametersCount { get; }

    public InvalidTypeTask(string assemblyQualifiedName, int parametersCount) : base(false)
    {
        AssemblyQualifiedName = assemblyQualifiedName;
        ParametersCount = parametersCount;

        Init();
    }

    protected override void OnInit()
    {
        for (int i = 0; i < ParametersCount; i++)
        {
            Parameters.Add(new ParameterString($"U{i}", "", ""));
        }
    }

    protected override void OnExecute()
    {
        throw new NotImplementedException();
    }

    public override InvalidTypeTask Clone()
    {
        var clone = new InvalidTypeTask(AssemblyQualifiedName, ParametersCount);

        var keys = Parameters.Keys;
        foreach (var key in keys)
        {
            clone.Parameters[key] = Parameters[key];
        }

        return clone;
    }

    public override Token[] ToTokens()
    {
        string name = AssemblyQualifiedName.Split(',', 2)[0];
        var values = new List<string>();
        foreach ( var para in Parameters)
        {
            values.Add(para.Value);
        }

        var tokens = new List<Token>();
        tokens.Add(new Token(TokenType.Error, $"!{name}"));
        tokens.Add(new Token(TokenType.Text, $"("));
        for (int i = 0;i < ParametersCount; i++)
        {
            tokens.Add(new Token(TokenType.Variable, values[i]));
            if (i < ParametersCount - 1)
            {
                tokens.Add(new Token(TokenType.Text, ","));
            }
        }
        tokens.Add(new Token(TokenType.Text, $")"));

        return tokens.ToArray();
    }
}
