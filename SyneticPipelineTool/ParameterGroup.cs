using GGL.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool;

public class ParameterGroup : IEnumerable<Parameter>, IViewObject
{
    public bool IsSealed { get; private set; } = false;

    private List<string> keys = new();
    private List<Parameter> values = new();

    public void Def<T>(string name, string desc = "", string value = "", object args = null) where T : Parameter
    {

    }

    public void Def(ParameterTypes type, string name, string desc = "", string value = "", object args = null)
    {
        if (IsSealed == true)
            throw new InvalidOperationException();

        Add(ParameterFactory.Create(type, name, desc, value, args));
    }

    public void Add(Parameter parameter)
    {
        if (IsSealed == true)
            throw new InvalidOperationException();

        if (keys.Contains(parameter.Name))
            throw new InvalidOperationException();

        keys.Add(parameter.Name);
        values.Add(parameter);
    }

    public void Add(params Parameter[] parameters)
    {
        foreach (var parameter in parameters)
        {
            Add(parameter);
        }
    }

    public void Seal()
    {
        IsSealed = true;
    }

    public string this[int index]
    {
        get
        {
            AssertSealed();
            return values[index].Value;
        }
        set
        {
            AssertSealed();
            values[index].Value = value;
        }
    }

    public string this[string name]
    {
        get
        {
            AssertSealed();
            return values[keys.IndexOf(name)].Value;
        }
        set
        {
            AssertSealed();
            values[keys.IndexOf(name)].Value = value;
        }
    }

    public string[] Keys => keys.ToArray();

    public int Count => keys.Count;


    public IEnumerator<Parameter> GetEnumerator() => values.GetEnumerator();

    public void ReadFromView(BinaryViewReader br)
    {
        AssertSealed();
        int count = br.ReadUInt16();
        var keys = Keys;
        for (int i = 0; i < count; i++)
        {
            string value = br.ReadString();
            if (i < keys.Length)
            {
                this[keys[i]] = value;
            }
        }
    }

    public void AssertSealed()
    {
        if (IsSealed == false)
            throw new InvalidOperationException();
    }

    public void WriteToView(BinaryViewWriter bw)
    {
        AssertSealed();
        bw.WriteUInt16((ushort)values.Count);
        foreach (var parameter in values)
            bw.WriteString(parameter.Value);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
