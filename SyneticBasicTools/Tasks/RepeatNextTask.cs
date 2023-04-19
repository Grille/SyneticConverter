﻿using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticPipelineTool.Tasks;

internal class RepeatNextTask : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.Integer, "Start", "", "1");
        Parameters.Def(ParameterTypes.Integer, "End", "", "3");
        Parameters.Def(ParameterTypes.String, "Variable", "", "i");
    }

    protected override void OnExecute()
    {
        int start = int.Parse(EvalParameter("Start"));
        int end = int.Parse(EvalParameter("End"));
        string variable = EvalParameter("Variable");

        var next = Pipeline.Tasks[Pipeline.TaskPosition += 1];
        if (next == null)
            throw new NullReferenceException();

        Console.WriteLine("start");
        for (int i = start; i <= end; i++)
        {
            Pipeline.Variables[variable] = i.ToString();
            next.Execute();
        }
        Console.WriteLine("end");
    }

    public override string ToString()
    {
        return $"Repeat next from {Parameters["Start"]} to {Parameters["End"]} as {Parameters["Variable"]}:";
    }
}