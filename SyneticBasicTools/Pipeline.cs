using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticBasicTools;

public class Pipeline : IViewObject
{
    public string Name;

    public PiplineList Owner;

    public Dictionary<string, string> Variables;
    public List<PipelineTask> Tasks { get; private set; }



    public Pipeline(PiplineList owner, string name)
    {
        Owner = owner;
        Name = name;
        Tasks = new List<PipelineTask>();
    }

    public void ReadFromView(BinaryViewReader br)
    {
        Tasks.Clear();
        int count = br.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            string typeName = br.ReadString();
            var task = CreateTask(typeName);
            Tasks.Add(task);
            br.ReadToIView(task);

        }

    }

    public void WriteToView(BinaryViewWriter bw)
    {
        bw.WriteInt32(Tasks.Count);
        for (int i = 0; i < Tasks.Count; i++)
        {

            var task = Tasks[i];
            bw.WriteString(task.GetType().AssemblyQualifiedName);
            bw.WriteIView(task);
        }
    }

    public void Execute()
    {
        Variables.Clear();

        foreach (var task in Tasks)
        {
            task.Execute();
        }
    }

    public string PathFromVar(string name)
    {
        return name;
    }

    public override string ToString()
    {
        return Name;
    }

    public Pipeline Clone()
    {
        string uname = Owner.GetUniqueName($"{Name} Clone");
        var clone = Owner.Create(uname);

        foreach (var task in Tasks)
        {
            task.CloneTo(clone);
        }

        return clone;
    }

    public PipelineTask CreateTask(string assemblyQualifiedName)
    {
        var type = Type.GetType(assemblyQualifiedName);
        return CreateTask(type);
    }

    public PipelineTask CreateTask(Type type)
    {
        var task = (PipelineTask)Activator.CreateInstance(type);
        task.Pipeline = this;
        return task;
    }

}
