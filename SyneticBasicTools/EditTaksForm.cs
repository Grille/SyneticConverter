using SyneticPipelineTool.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticPipelineTool;

public partial class EditTaksForm : Form
{
    public Pipeline Pipeline;
    public PipelineTask Task;
    Dictionary<string, Type> types;
    List<Control> inputs;
    Type displayedType = null;

    bool setup = true;

    public EditTaksForm(Pipeline pipeline, PipelineTask task = null)
    {
        InitializeComponent();
        DisplayParameters();

        inputs= new List<Control>();

        types = new Dictionary<string, Type>()
        {
            ["Comment"] = typeof(NopTask),
            ["< Logic >"] = typeof(NopTask),
            ["Variable"] = typeof(VariableOperationTask),
            ["Repeat Next"] = typeof(RepeatNextTask),
            ["Call pipeline"] = typeof(ExecutePipelineTask),
            ["< IO >"] = typeof(NopTask),
            ["Load file"] = typeof(LoadFileTask),
            ["Copy file"] = typeof(CopyFileTask),
            ["Copy directory"] = typeof(CopyDirTask),
            ["Clear directory"] = typeof(ClearDirTask),
            ["Remove directory"] = typeof(RemoveDirTask),
            ["< Tools >"] = typeof(NopTask),
            ["Convert scenario files"] = typeof(ConvertScnFilesTask),
            ["Convert MBWR sprites to WR2"] = typeof(FixTreeNormTask),
        };

        comboBoxType.BeginUpdate();
        foreach (var key in types.Keys)
        {
            comboBoxType.Items.Add(key);
        }
        comboBoxType.EndUpdate();

        Pipeline = pipeline;

        if (task != null)
            Task = task.Clone();
        else
            Task = new NopTask();

        DisplayType();
        DisplayParameters();

        setup = false;
    }

    public new DialogResult ShowDialog(IWin32Window owner)
    {
        return base.ShowDialog(owner);
    }

    public void DisplayType()
    {
        if (Task == null)
            return;

        foreach (var pair in types)
        {
            if (pair.Value == Task.GetType())
            {
                comboBoxType.SelectedItem = pair.Key;
                return;
            }
        }

        throw new KeyNotFoundException();
    }

    public void DisplayParameters()
    {
        var panel = panelParameters;

        if (Task == null)
        {
            panel.Controls.Clear();
            return;
        }

        var type = Task.GetType();
        panel.Controls.Clear();
        inputs.Clear();
        int posY = 0;
        foreach (var param in Task.Parameters)
        {
            var label = new Label();
            label.Text = $"{param.Name}:";
            label.Top = posY + 2;
            label.Width = 100;

            Control input = param.CreateControl();

            input.Left = label.Width;
            input.Width = panel.Width - label.Width - 10;

            input.Top = posY;
            input.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            input.TextChanged += Input_TextChanged;
            inputs.Add(input);

            panel.Controls.Add(label);
            panel.Controls.Add(input);

            if (param.Description != "")
            {
                var desc = new Label();
                desc.Text = param.Description;
                posY += 25;
                desc.Top = posY;
                desc.Width = panel.Width;
                desc.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                panel.Controls.Add(desc);
            }

            posY += 30;
        }

        ApplyInputs();
    }

    private void Input_TextChanged(object sender, EventArgs e)
    {
        ApplyInputs();
    }

    public void ApplyInputs()
    {
        int i = 0;
        foreach (var param in Task.Parameters)
        {
            param.Value = inputs[i++].Text;
        }
        textBox.Text = Task.ToString();
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
        ApplyInputs();
        DialogResult = DialogResult.OK;
        Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
        DialogResult= DialogResult.Cancel;
        Close();
    }

    private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (setup)
            return;

        var type = types[(string)comboBoxType.SelectedItem];
        Task = Pipeline.Tasks.CreateUnbound(type);

        DisplayParameters();
    }
}
