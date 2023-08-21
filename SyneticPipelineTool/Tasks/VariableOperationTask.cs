﻿using GGL.IO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool.Tasks;

[PipelineTask(Name = "Variable")]
internal class VariableOperationTask : PipelineTask
{

    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Name", "", "Var");
        Parameters.Def(ParameterTypes.Enum, "Operator", "", "=",new string[] {"=", "+" });
        Parameters.Def(ParameterTypes.String, "Value", "", "Value");
    }

    protected override void OnExecute()
    {
        var op = Parameters["Operator"].ToLower();
        var name = EvalParameter("Name");
        var value = EvalParameter("Value");
        switch (op)
        {
            case "=":
            {
                Runtime.Variables[name] = value;
                break;
            }
            case "+":
            {
                Runtime.Variables[name] += value;
                break;
            }
            case "Replace":
            {
                Runtime.Variables[name] += value;
                break;
            }
            default:
            {
                throw new ArgumentOutOfRangeException();
            }
        }
        Console.WriteLine($"{name} {op} {value} -> {Runtime.Variables[name]}");
    }

    public override string ToString()
    {
        return $"{Parameters["Name"]} {Parameters["Operator"]} {Parameters["Value"]}";
    }
}