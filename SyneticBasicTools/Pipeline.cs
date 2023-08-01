using GGL.IO;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.PortableExecutable;
using SyneticPipelineTool.Tasks;

namespace SyneticPipelineTool;

public class Pipeline : IViewObject
{
    const int Magic = 23658;
    public string Name;

    public PipelineList Owner;

    public Dictionary<string, string> Variables;
    public int TaskPosition { get; set; }

    public PipelineTaskList Tasks { get; private set; }

    public Stack<Pipeline> CallStack { get; private set; }



    public Pipeline(PipelineList owner, string name)
    {
        Owner = owner;
        Name = name;
        Variables = new();
        Tasks = new(this);
        TaskPosition = 0;
    }

    public void ReadFromView(BinaryViewReader br)
    {
        int magic = br.ReadInt32();
        if (magic != Magic)
            throw new InvalidDataException();

        br.LengthPrefix = LengthPrefix.UInt16;
        br.Encoding = Encoding.UTF8;

        int typeCount = br.ReadInt32();
        var types = new List<FileTaskTypeInfo>();
        for (int i = 0; i < typeCount; i++)
        {
            types.Add(br.ReadIView<FileTaskTypeInfo>());
        }

        Tasks.Clear();
        int count = br.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            int typeId = br.ReadInt32();
            var task = types[typeId].Create();
            Tasks.Link(task);
            br.ReadToIView(task);
        }

    }

    public void WriteToView(BinaryViewWriter bw)
    {
        bw.WriteInt32(Magic);

        bw.LengthPrefix = LengthPrefix.UInt16;
        bw.Encoding = Encoding.UTF8;

        var types = FileTaskTypeInfo.GetTypes(Tasks);

        bw.WriteInt32(types.Count);
        foreach (var type in types)
            bw.WriteIView(type);

        bw.WriteInt32(Tasks.Count);
        for (int i = 0; i < Tasks.Count; i++)
        {
            var task = Tasks[i];
            bw.WriteInt32(FileTaskTypeInfo.IndexOf(types, task));
            bw.WriteIView(task);
        }
    }

    public void Execute(Stack<Pipeline> callStack)
    {

        if (callStack.Contains(this))
            throw new InvalidOperationException($"Pipeline already in call stack.");

        CallStack = callStack;

        try
        {
            CallStack.Push(this);

            for (TaskPosition = 0; TaskPosition < Tasks.Count; TaskPosition++)
            {
                Tasks[TaskPosition].Execute();
            }

            CallStack.Pop();
        }
        finally
        {
            Variables.Clear();

            CallStack = null;
        }
    }

    public override string ToString()
    {
        return Name;
    }

    public Pipeline Clone()
    {
        string uname = Owner.GetUniqueName($"{Name} Clone");
        var clone = Owner.CreateUnbound(uname);

        foreach (var task in Tasks)
        {
            task.CloneTo(clone);
        }

        return clone;
    }
}

internal class FileTaskTypeInfo : IViewObject
{
    public string AssemblyQualifiedName;
    public int ParametersCount;

    public FileTaskTypeInfo()
    {

    }

    public FileTaskTypeInfo(PipelineTask task)
    {
        var type = task.GetType();
        AssemblyQualifiedName = type.AssemblyQualifiedName;
        ParametersCount = task.Parameters.Count;
    }

    public static int IndexOf(List<FileTaskTypeInfo> list, PipelineTask task)
    {
        if (task is InvalidTypeTask)
        {
            var itask = (InvalidTypeTask)task;
            return list.FindIndex((a) => a.AssemblyQualifiedName == itask.AssemblyQualifiedName);
        }
        else
        {
            return list.FindIndex((a) => a.AssemblyQualifiedName == task.GetType().AssemblyQualifiedName);
        }
    }

    public static bool Contains(List<FileTaskTypeInfo> list, PipelineTask task)
    {
        return IndexOf(list, task) != -1;
    }

    public static List<FileTaskTypeInfo> GetTypes(PipelineTaskList tasks)
    {
        var types = new List<FileTaskTypeInfo>();
        for (int i = 0; i < tasks.Count; i++)
        {
            var task = tasks[i];
            if (task is InvalidTypeTask)
            {
                var itask = (InvalidTypeTask)task;
                if (!Contains(types, tasks[i]))
                {
                    types.Add(new()
                    {
                        AssemblyQualifiedName = itask.AssemblyQualifiedName,
                        ParametersCount = itask.Parameters.Count,
                    });
                }
            }
            else
            {
                if (!Contains(types, tasks[i]))
                {
                    types.Add(new(tasks[i]));
                }
            }
        }
        return types;
    }

    public PipelineTask Create()
    {
        var type = Type.GetType(AssemblyQualifiedName);
        if (type == null)
        {
            Console.WriteLine(AssemblyQualifiedName);
            Console.WriteLine(ParametersCount);
            return new InvalidTypeTask(AssemblyQualifiedName, ParametersCount);
        }
        else
        {
            return (PipelineTask)Activator.CreateInstance(type);
        }
    }

    public void ReadFromView(BinaryViewReader br)
    {
        AssemblyQualifiedName = br.ReadString();
        ParametersCount = br.ReadByte();
    }

    public void WriteToView(BinaryViewWriter bw)
    {
        bw.WriteString(AssemblyQualifiedName);
        bw.WriteByte((byte)ParametersCount);
    }
}