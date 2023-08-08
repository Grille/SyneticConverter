using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool;

public class PipelineTaskList : List<PipelineTask>
{
    public readonly Pipeline Pipeline;

    public PipelineTaskList(Pipeline pipeline) { 
        Pipeline = pipeline;
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
