using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticPipelineTool;


public static class ParameterTypes
{
    public readonly static ParameterTypeString String = new();
    public readonly static ParameterTypeInteger Integer = new();
    public readonly static ParameterTypeEnum Enum = new();
}

public abstract class ParameterType
{
    public abstract void AssertValid(in object args);

    public abstract bool ValidateValue(in object args, in string value);

    public abstract Control CreateControl(in object args, in string value);
}

public class ParameterTypeString : ParameterType
{
    public override void AssertValid(in object args)
    {
        return;
    }

    public override bool ValidateValue(in object args, in string value)
    {
        return true;
    }

    public override Control CreateControl(in object args, in string value)
    {
        var obj = new TextBox();
        obj.Text = value;
        return obj;
    }
}

public class ParameterTypeInteger : ParameterType
{
    public override void AssertValid(in object args)
    {
        return;
    }

    public override bool ValidateValue(in object args, in string value)
    {
        return int.TryParse(value, out _);
    }

    public override Control CreateControl(in object args, in string value)
    {
        var obj = new TextBox();
        obj.Text = value;
        return obj;
    }
}

public class ParameterTypeEnum : ParameterType
{
    public override void AssertValid(in object args)
    {
        if (!typeof(string[]).IsInstanceOfType(args))
            throw new ArgumentException();
        return;
    }

    public override bool ValidateValue(in object args, in string value)
    {
        return ((string[])args).Contains(value);
    }

    public override Control CreateControl(in object args, in string value)
    {
        var obj = new ComboBox();
        obj.BeginUpdate();
        foreach (var it in (string[])args)
        {
            obj.Items.Add(it);
        }
        obj.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        obj.SelectedItem = value;
        obj.EndUpdate();
        return obj;
    }
}
