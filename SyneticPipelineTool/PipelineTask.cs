using GGL.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool;

public abstract class PipelineTask : IViewObject
{
    public Pipeline Pipeline { get; set; }

    public ParameterGroup Parameters { get; } = new();

    protected Runtime Runtime { get; private set; }

    protected PipelineTask(bool autoinit)
    {
        if (autoinit)
            Init();
    }

    public PipelineTask()
    {
        Init();
    }

    public void Init()
    {
        OnInit();

        if (Parameters.Count == 0)
        {
            var infos = GetType().GetProperties();

            foreach (var info in infos)
            {
                if (info.PropertyType.IsAssignableTo(typeof(Parameter)))
                {
                    var obj = (Parameter)info.GetValue(this);

                    if (obj == null)
                        throw new NullReferenceException();

                    Parameters.Add(obj);
                }

            }
        }

        Parameters.Seal();
    }

    protected abstract void OnInit();

    public void Execute(Runtime runtime)
    {
        Parameters.AssertSealed();
        Runtime = runtime;
        OnExecute();
    }

    public void Update() => OnUpdate();
    protected virtual void OnUpdate()
    {

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

    public virtual PipelineTask Clone()
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

    protected string EvalParameter(Parameter parameter)
    {
        return Runtime.EvalParameter(parameter);
    }
    protected string EvalParameter(in string name)
    {
        var value = Parameters[name];
        return Runtime.EvalParameterValue(value);
    }
}
