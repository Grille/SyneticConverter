using GGL.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool;

public abstract class PipelineTask : IViewObject
{
    public Pipeline Pipeline;

    public ParameterGroup Parameters = new();

    public PipelineTask()
    {
        Init();
    }

    public void Init()
    {
        OnInit();
        Parameters.Seal();
    }

    protected abstract void OnInit();

    public void Execute()
    {
        try
        {
            Parameters.AssertSealed();
            OnExecute();
        }
        catch (Exception err)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(err.Message);
            Console.ForegroundColor = ConsoleColor.Gray;
            throw;
        }
    }

    protected abstract void OnExecute();

    public void ReadFromView(BinaryViewReader br)
    {
        br.ReadToIView(Parameters);
    }
    public void WriteToView(BinaryViewWriter bw)
    {
        bw.WriteIView(Parameters);
    }

    public virtual void Validate()
    {

    }

    public PipelineTask Clone()
    {
        var type = GetType();
        var clone = Pipeline.Tasks.CreateUnbound(type);

        var keys = Parameters.Keys;
        foreach (var key in keys)
        {
            clone.Parameters[key] = Parameters[key];
        }

        return clone;
    }

    public void CloneTo(Pipeline target)
    {
        var clone = Clone();
        clone.Pipeline = target;
        target.Tasks.Add(clone);
    }

    protected string GetValue(in string name)
    {
        return parseValue(Parameters[name]);
    }

    private string parseValue(in string value)
    {
        if (value.Length == 0)
            return value;

        if (value[0] == '*')
        {
            var key = parseValue(value.Substring(1));
            return Pipeline.Variables[key];
        }
        if (value[0] == '$')
        {
            var exp = parseValue(value.Substring(1));

            var list = new List<string>();
            var split0 = exp.Split("{");
            foreach (var s0 in split0)
            {
                var split1 = s0.Split("}", 2);
                list.Add(split1[0]);
                if (split1.Length > 1)
                    list.Add(split1[1]);
            }

            string result = "";
            for (int i = 0; i < list.Count; i++)
            {
                if (i % 2 == 0)
                    result += list[i];
                else
                    result += parseValue(list[i]);
            }

            return result;
        }

        return value;
    }
}
