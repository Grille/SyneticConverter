using GGL.IO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticBasicTools.Tasks;

internal class VariableOperationTask : PipelineTask
{

    protected override void OnInit()
    {
        Parameters.Def(ParamType.String, "Name", "", "Name");
        Parameters.Def(ParamType.String, "Operator", "(Set)=, (Add)+", "=");
        Parameters.Def(ParamType.String, "Value", "", "Value");
    }

    protected override void OnExecute()
    {
        var op = Parameters["Operator"].ToLower();
        var name = GetValue("Name");
        var value = GetValue("Value");
        switch (op)
        {
            case "set":
            case "=":
            {
                Pipeline.Variables[name] = value;
                break;
            }
            case "add":
            case "+":
            {
                Pipeline.Variables[name] += value;
                break;
            }
            default:
            {
                throw new ArgumentOutOfRangeException();
            }
        }
        Console.WriteLine($"{name} {op} {value} -> {Pipeline.Variables[name]}");
    }

    public override string ToString()
    {
        return $"{Parameters["Name"]} {Parameters["Operator"]} {Parameters["Value"]}";
    }
}
