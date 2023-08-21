using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool;

using Variables = Dictionary<string, string>;

public class Runtime
{
    public class CallStackEntry
    {
        public Pipeline Pipeline { get; }
        public int Position { get; set; }

        public CallStackEntry(Pipeline pipeline)
        {
            Pipeline = pipeline;
            Position = 0;
        }

        public static implicit operator CallStackEntry(Pipeline pipeline) => new CallStackEntry(pipeline);
    }

    public Pipeline Pipeline => CallStack.Peek().Pipeline;

    public int Position
    {
        get => CallStack.Peek().Position;
        set => CallStack.Peek().Position = value;
    }

    public Stack<CallStackEntry> CallStack { get; }

    public Stack<Variables> ScopeStack { get; }

    public Variables Variables => ScopeStack.Peek();

    public string StackTrace
    {
        get
        {
            var list = CallStack.ToList();
            list.Reverse();
            string stackTrace = "";
            foreach (var item in list)
            {
                stackTrace += $"Pipeline: {item.Pipeline.Name}, Line: {item.Position + 1}\n";
            }
            return stackTrace;
        }
    }

    public Runtime()
    {
        CallStack = new();
        ScopeStack = new();
    }

    public void Clear()
    {
        CallStack.Clear();
        ScopeStack.Clear();
    }

    public void Call(Pipeline pipeline)
    {
        if (CallStack.Any(a => a.Pipeline == pipeline))
            throw new InvalidOperationException($"Pipeline already in call stack.");

        CallStack.Push(pipeline);
        IncVariableScope();

        Execute(pipeline);

        DecVariableScope();
        CallStack.Pop();
    }

    public void Execute(Pipeline pipeline)
    {
        var tasks = pipeline.Tasks;
        int count = tasks.Count;

        while (Position < count)
        {
            tasks[Position].Execute(this);
            Position += 1;
        }
    }

    public string EvalParameter(Parameter parameter)
    {
        return EvalParameterValue(parameter.Value);
    }

    public void IncVariableScope()
    {
        var variables = new Variables();
        if (ScopeStack.Count > 0)
        {
            var curent = ScopeStack.Peek();
            foreach ( var pair in curent)
            {
                variables[pair.Key] = pair.Value;
            }
        }
        ScopeStack.Push(variables);
    }

    public void DecVariableScope()
    {
        ScopeStack.Pop();
    }

    public string EvalParameterValue(string value)
    {
        if (value.Length == 0)
            return value;

        if (value[0] == '*')
        {
            var key = EvalParameterValue(value.Substring(1));
            if (!Variables.ContainsKey(key))
            {
                throw new InvalidOperationException($"Variable '{key}' not found.");
            }
            return Variables[key];
        }
        if (value[0] == '$')
        {
            var exp = EvalParameterValue(value.Substring(1));

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
                    result += EvalParameterValue(list[i]);
            }

            return result;
        }

        return value;
    }
}
