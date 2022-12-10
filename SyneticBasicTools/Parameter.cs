using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticBasicTools;

public class Parameter
{
    public readonly string Name;
    public readonly string Type;
    public readonly string Description;
    public string Value;

    public Parameter(string name, string type, string desc = "", string value = "")
    {
        Name = name;
        Description = desc;
        Type = type;
        Value = value;
    }
}
