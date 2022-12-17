using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticBasicTools;

public class Parameter
{
    public readonly ParamType Type;
    public readonly string Name;
    public readonly string Description;
    public string Value;

    public Parameter(ParamType type, string name, string desc = "", string value = "")
    {
        Name = name;
        Description = desc;
        Type = type;
        Value = value;
    }
}

public enum ParamType
{
    String,
    Path,
    Version,
}
