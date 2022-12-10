using GGL.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticBasicTools;

public class ParameterGroup : IEnumerable<Parameter>, IViewObject
{
    private bool isSealed = false;
    private Dictionary<string, Parameter> parameters = new();

    public void Def(string name, string type, string desc = "", string value = "")
    {
        if (isSealed == true)
            throw new InvalidOperationException();

        var parameter = new Parameter(name, type, desc, value);
        parameters.Add(name, parameter);
    }

    public void Seal()
    {
        isSealed = true;
    }

    public string this[string name]
    {
        get
        {
            AssertSealed();
            return parameters[name].Value;
        }
        set
        {
            AssertSealed();
            parameters[name].Value = value;
        }
    }


    public string[] Keys => parameters.Keys.ToArray();


    public IEnumerator<Parameter> GetEnumerator() => parameters.Values.GetEnumerator();

    public void ReadFromView(BinaryViewReader br)
    {
        AssertSealed();

        foreach (var parameter in parameters.Values)
            parameter.Value = br.ReadString();
    }

    public void AssertSealed()
    {
        if (isSealed == false)
            throw new InvalidOperationException();
    }

    public void WriteToView(BinaryViewWriter bw)
    {
        AssertSealed();

        foreach (var parameter in parameters.Values)
            bw.WriteString(parameter.Value);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
