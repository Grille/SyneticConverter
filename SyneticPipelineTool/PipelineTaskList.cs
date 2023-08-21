using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool;

public class PipelineTaskList : List<PipelineTask>
{
    public Pipeline Pipeline { get; }

    public PipelineTaskList(Pipeline pipeline) { 
        Pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
    }

    public new void Add(PipelineTask task)
    {
        if (task.Pipeline != Pipeline)
            throw new InvalidOperationException();

        base.Add(task);
    }

    public PipelineTask CreateUnbound(string assemblyQualifiedName)
    {
        var type = Type.GetType(assemblyQualifiedName);
        return CreateUnbound(type);
    }

    public PipelineTask CreateUnbound(Type type)
    {
        var task = (PipelineTask)Activator.CreateInstance(type);
        task.Pipeline = Pipeline;
        return task;
    }

    public PipelineTask Create(string assemblyQualifiedName)
    {
        var task = CreateUnbound(assemblyQualifiedName);
        Add(task);
        return task;
    }

    public PipelineTask Create(Type type)
    {
        var task = CreateUnbound(type);
        Add(task);
        return task;
    }

    public void Link(PipelineTask task)
    {
        task.Pipeline = Pipeline;
        Add(task);
    }
}
