using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticPipelineTool;

public static class ParameterFactory
{
    public static Parameter Create(ParameterTypes type, string name, string desc, string value, object args) => type switch
    {
        ParameterTypes.Enum => new ParameterEnum(name, desc, value, (string[])args),
        ParameterTypes.Integer => new ParameterInteger(name, desc, value),
        ParameterTypes.Single => new ParameterSingle(name, desc, value),
        ParameterTypes.Boolean => new ParameterBoolean(name, desc, value),
        ParameterTypes.String => new Parameter(name, desc, value),
        _ => throw new NotImplementedException()
    };
}

public class Parameter
{
    public readonly string Name;
    public readonly string Description;
    public bool Enabled = true;
    public string Value;

    public Parameter(string name, string desc, string value)
    {
        Name = name;
        Description = desc;
        Value = value;
    }

    public virtual bool ValidateValue()
    {
        return true;
    }

    public virtual Control CreateControl()
    {
        var obj = new TextBox();
        obj.Text = Value;
        return obj;
    }

    public static List<string> ValueToList(string value)
    {
        var array = value.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
        var list = new List<string>();
        foreach (var item in array)
        {
            list.Add(item.Trim());
        }
        return list;
    }
}

public class ParameterInteger : Parameter
{
    public ParameterInteger(string name, string desc, string value) : base(name, desc, value)
    {
    }

    public override bool ValidateValue()
    {
        return int.TryParse(Value, out _);
    }
}

public class ParameterSingle : Parameter
{
    public ParameterSingle(string name, string desc, string value) : base(name, desc, value)
    {
    }

    public override bool ValidateValue()
    {
        return float.TryParse(Value, out _);
    }
}

public class ParameterBoolean : ParameterEnum
{
    public ParameterBoolean(string name, string desc, string value) : base(name, desc, value, new string[] { "false", "true"})
    {
    }
}

public class ParameterEnum : Parameter
{
    public readonly string[] Args;
    public ParameterEnum(string name, string desc, string value, string[] args) : base(name, desc, value)
    {
        Args = args;
    }

    public override bool ValidateValue()
    {
        return Args.Contains(Value);
    }

    public override Control CreateControl()
    {
        var obj = new ComboBox();
        obj.BeginUpdate();
        foreach (var it in Args)
        {
            obj.Items.Add(it);
        }
        obj.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        if (ValidateValue())
            obj.SelectedItem = Value;
        else
            obj.SelectedText = Value;
        obj.EndUpdate();
        return obj;
    }
}

public enum ParameterTypes
{
    String,
    Integer,
    Single,
    Boolean,
    Enum,
}
