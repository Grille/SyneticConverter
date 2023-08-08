using GGL.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SyneticPipelineTool;

public class PipelineList : List<Pipeline>, IViewObject
{
    public string Path = "pipelines.dat";

    public void ReadFromView(BinaryViewReader br)
    {
        br.LengthPrefix = LengthPrefix.UInt16;
        br.Encoding = Encoding.UTF8;

        int count = br.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            string name = br.ReadString();
            var pipeline = CreateUnbound(name);
            Add(pipeline);
            br.ReadToIView(pipeline);
        }
    }

    public void WriteToView(BinaryViewWriter bw)
    {
        bw.LengthPrefix = LengthPrefix.UInt16;
        bw.Encoding = Encoding.UTF8;

        bw.WriteInt32(Count);
        for (int i = 0; i < Count; i++)
        {
            var pipeline = this[i];
            bw.WriteString(pipeline.Name);
            bw.WriteIView(pipeline);
        }
    }

    public void Save()
    {
        using var bw = new BinaryViewWriter(Path);
        bw.WriteIView(this);
    }

    public void Load()
    {
        using var br = new BinaryViewReader(Path);
        br.ReadToIView(this);
    }

    public bool TryLoad()
    {
        if (!File.Exists(Path))
            return false;

        try
        {
            Load();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Pipeline this[string name]
    {
        get => Find(x => x.Name == name);
    }

    public bool ContainsName(string name)
    {
        return FindIndex(x => x.Name == name) != -1;
    }

    public Pipeline CreateUnbound(string name)
    {
        if (ContainsName(name))
            throw new InvalidOperationException();

        var pipeline = new Pipeline(this, name);
        pipeline.Owner = this;
        return pipeline;
    }

    public void Rename(string name, string newname)
    {
        if (!ContainsName(name))
            throw new InvalidOperationException();

        if (name == newname)
            return;

        if (ContainsName(newname))
            throw new InvalidOperationException();

        this[name].Name= newname;
    }

    public string GetUniqueName(string name)
    {
        if (!ContainsName(name))
            return name;

        int i = 1;
        while (true)
        {
            string uname = $"{name} #{i++}";
            if (!ContainsName(uname))
                return uname;
        }
    }
}
