using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticPipelineTool;

public class Parameter
{
    protected readonly ParameterType Type;
    public readonly string Name;
    public readonly string Description;
    public readonly object Args;
    public bool Enabled = true;
    public string Value;

    public Parameter(ParameterType type, string name, string desc, string value, object args)
    {
        Name = name;
        Description = desc;
        Type = type;
        Value = value;
        Args = args;

        AssertValid();
    }

    public void AssertValid()
    {
        Type.AssertValid(Args);
    }

    public bool ValidateValue()
    {
        return Type.ValidateValue(Args, Value);
    }

    public Control CreateControl()
    {
        return Type.CreateControl(Args, Value);
    }
}

public enum ParamType
{
    String,
    Path,
    Enum,
    Version,
    List,
}
