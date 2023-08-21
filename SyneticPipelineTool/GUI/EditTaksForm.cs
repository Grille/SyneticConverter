﻿using SyneticPipelineTool.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticPipelineTool;

public partial class EditTaksForm : Form
{
    public Pipeline Pipeline { get; }
    public PipelineTask Task { get; private set; }

    static SortedList<string, Type> types;
    List<Control> inputs;

    bool setup = true;

    bool keepValues = false;

    public EditTaksForm(Pipeline pipeline, PipelineTask task = null)
    {
        if (pipeline == null)
            throw new ArgumentNullException(nameof(pipeline));

        inputs = new List<Control>();

        InitializeComponent();
        DisplayParameters();

        InitTypes();

        comboBoxType.BeginUpdate();
        foreach (var key in types.Keys)
        {
            comboBoxType.Items.Add(key);
        }
        comboBoxType.EndUpdate();

        Pipeline = pipeline;

        if (task is InvalidTypeTask)
            keepValues = true;
        if (task != null)
            Task = task.Clone();
        else
            Task = new NopTask();

        DisplayType();
        DisplayParameters();

        setup = false;
    }

    static void InitTypes()
    {
        if (types != null)
            return;

        types = new SortedList<string, Type>()
        {
            ["Comment"] = typeof(NopTask),
        };

        var asm = Assembly.GetExecutingAssembly();
        var asmTypes = asm.GetTypes();

        foreach (var type in asmTypes)
        {
            var attr = type.GetCustomAttribute<PipelineTaskAttribute>();
            if (attr == null)
                continue;

            var name = attr.Name;

            if (name == null)
                name = type.Name;

            types[name] = type;
        }
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

        //throw new KeyNotFoundException();
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
            var input = inputs[i];
            param.Value = inputs[i].Text;

            if (param.Value.Length > 0 && (param.Value[0] == '$' || param.Value[0] == '*'))
            {
                input.ForeColor = Color.Blue;
            }
            else if (param.ValidateValue())
            {
                input.ForeColor = Color.Black;
            }
            else
            {
                input.ForeColor = Color.Red;
            }

            i++;
        }
        textBox.Text = Task.ToString();

        Task.Update();
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
        ApplyInputs();
        DialogResult = DialogResult.OK;
        Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (setup)
            return;

        var oldTask = Task;

        var type = types[(string)comboBoxType.SelectedItem];
        Task = Pipeline.Tasks.CreateUnbound(type);

        if (keepValues)
        {
            int count = Math.Min(oldTask.Parameters.Count, Task.Parameters.Count);
            for (int i = 0; i < count; i++)
            {
                Task.Parameters[i] = oldTask.Parameters[i];
            }
        }

        DisplayParameters();
    }
}