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
    //public bool Running { get; private set; }

    private bool cancel;

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
        set
        {
            CallStack.Peek().Position = value;
            InvPosChanged();
        }
    }

    public Stack<CallStackEntry> CallStack { get; }

    public Stack<Variables> ScopeStack { get; }

    public Stack<string> ValueStack { get; }

    public Variables Variables => ScopeStack.Peek();

    public event EventHandler PositionChanged;


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
        ValueStack = new();
    }

    private void InvPosChanged()
    {
        PositionChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Clear()
    {
        cancel = false;
        CallStack.Clear();
        ScopeStack.Clear();
        ValueStack.Clear();
        InvPosChanged();
    }

    public void Cancel()
    {
        cancel = true;
    }

    public void Call(Pipeline pipeline) 
    {
        if (CallStack.Any(a => a.Pipeline == pipeline))
            throw new InvalidOperationException($"Pipeline already in call stack.");

        if (cancel)
            return;

        CallStack.Push(pipeline);
        InvPosChanged();
        IncVariableScope();

        ExecuteTasks(pipeline);

        DecVariableScope();
        CallStack.Pop();
        InvPosChanged();
    }

    public void ExecuteTasks(Pipeline pipeline)
    {
        var tasks = pipeline.Tasks;
        int count = tasks.Count;

        while (Position < count && !cancel)
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

    public void Output(string name)
    {

    }

    public void Return()
    {

    }

    public void Break()
    {

    }

    public void ExecuteNextBlock()
    {
        var tasks = Pipeline.Tasks;

        var caller = Position;
        int start = Position + 1;
        int length = NextBlockLength();
        int end = start + length;

        for ( int i = start; i < end; i++)
        {
            Position = i;
            tasks[Position].Execute(this);
        }

        Position = caller;
    }

    public void SkipNextBlock()
    {
        int length = NextBlockLength();

        Position += length;
    }

    public int NextBlockLength()
    {
        var tasks = Pipeline.Tasks;
        int scope = tasks[Position].Scope;

        int start = Position + 1;
        var next = tasks[start];
        if (next == null)
            throw new NullReferenceException();

        if (next.Scope <= scope)
        {
            throw new Exception("Increased scope block expected.");
        }

        int count = 0;

        while (true)
        {
            int idx = count + start;

            if (idx >= tasks.Count)
                return count;

            if (tasks[idx].Scope <= scope) {
                return count;
            }

            count += 1;
        }
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
