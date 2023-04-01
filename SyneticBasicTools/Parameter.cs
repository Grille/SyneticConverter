using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticPipelineTool;

public class Parameter
{
    public readonly ParamType Type;
    public readonly string Name;
    public readonly string Description;
    public readonly object Args;
    public string Value;

    public Parameter(ParamType type, string name, string desc, string value, object args)
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
        switch (Type)
        {
            case ParamType.Enum:
                if (!typeof(string[]).IsInstanceOfType(Args))
                    throw new ArgumentException();
                return;
        }
    }

    public bool ValidateValue()
    {
        switch (Type)
        {
            case ParamType.Enum:
                return ((string[])Args).Contains(Value);
        }
        return true;
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
