using GGL.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticBasicTools;

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
        Parameters.AssertSealed();
        OnExecute();
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
        var clone = Pipeline.CreateTask(type);

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
}
