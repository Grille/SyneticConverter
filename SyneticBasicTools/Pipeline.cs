using GGL.IO;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticBasicTools;

public class Pipeline : IViewObject
{
    public string Name;

    public PipelineList Owner;

    public Dictionary<string, string> Variables;
    public PipelineTaskList Tasks { get; private set; }



    public Pipeline(PipelineList owner, string name)
    {
        Owner = owner;
        Name = name;
        Variables = new();
        Tasks = new(this);
    }

    public void ReadFromView(BinaryViewReader br)
    {
        int magic = br.ReadInt32();
        if (magic != 23658)
            throw new InvalidDataException();

        br.DefaultLengthPrefix = LengthPrefix.UInt16;
        br.DefaultCharSize = CharSize.Byte;

        int typeCount = br.ReadInt32();
        var types = new List<Type>();
        for (int i = 0; i < typeCount; i++)
        {
            var name = br.ReadString();
            var type = Type.GetType(name);
            types.Add(type);
        }

        Tasks.Clear();
        int count = br.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            int typeId = br.ReadInt32();
            var task = Tasks.Create(types[typeId]);
            br.ReadToIView(task);
        }

    }

    public void WriteToView(BinaryViewWriter bw)
    {
        bw.WriteInt32(23658);

        bw.DefaultLengthPrefix = LengthPrefix.UInt16;
        bw.DefaultCharSize = CharSize.Byte;

        var types = new List<Type>();
        for (int i = 0; i < Tasks.Count; i++)
        {
            var type = Tasks[i].GetType();
            if (!types.Contains(type))
                types.Add(type);
        }

        bw.WriteInt32(types.Count);
        foreach (var type in types)
            bw.WriteString(type.AssemblyQualifiedName);

        bw.WriteInt32(Tasks.Count);
        for (int i = 0; i < Tasks.Count; i++)
        {
            var task = Tasks[i];
            bw.WriteInt32(types.IndexOf(task.GetType()));
            bw.WriteIView(task);
        }
    }

    public void Execute()
    {
        foreach (var task in Tasks)
        {
            task.Execute();
        }
        Variables.Clear();
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
