using SyneticPipelineTool.GUI;
using SyneticPipelineTool.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticPipelineTool;

public partial class EditTaksForm : Form
{
    public Pipeline Pipeline { get; private set; }
    public PipelineTask Task { get; private set; }
    public PipelineTask OriginalTask { get; private set; }

    List<Control> inputs;

    bool setup = true;

    bool keepValues = false;

    public EditTaksForm()
    {
        InitializeComponent();
        InitTypeTree();

        inputs = new List<Control>();
    }

    private void InitTypeTree()
    {
        treeViewTypes.BeginUpdate();
        treeViewTypes.Nodes.Clear();
        AssemblyTaskTypeTree.ApplyTo(treeViewTypes.Nodes);
        treeViewTypes.EndUpdate();
    }

    public void Init(Pipeline pipeline, PipelineTask task = null)
    {
        if (pipeline == null)
            throw new ArgumentNullException(nameof(pipeline));

        setup = true;
        keepValues = false;

        Pipeline = pipeline;
        OriginalTask = task;

        if (task is InvalidTypeTask)
            keepValues = true;
        if (task != null)
            Task = task.Clone();
        else
            Task = new NopTask();

        SelectCurrentType();
        DisplayParameters();

        setup = false;
    }

    public DialogResult ShowDialog(IWin32Window owner, Pipeline pipeline, PipelineTask task = null)
    {
        Init(pipeline, task);
        return ShowDialog();
    }

    public void SelectCurrentType()
    {
        if (Task == null)
        {
            treeViewTypes.SelectedNode = null;
            return;
        }

        var type = Task.GetType();

        if (type == typeof(InvalidTypeTask))
        {
            treeViewTypes.SelectedNode = null;
            return;
        }

        treeViewTypes.SelectedNode = AssemblyTaskTypeTree.GetTypeInfo(type);

        textBoxType.Text = treeViewTypes.SelectedNode.ToString();
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

    private void treeViewTypes_AfterSelect(object sender, TreeViewEventArgs e)
    {
        if (setup)
            return;

        var node = treeViewTypes.SelectedNode;

        if (node is not AssemblyTaskTypeTree.TypeInfo)
            return;

        var entry = (AssemblyTaskTypeTree.TypeInfo)treeViewTypes.SelectedNode;

        var type = entry.Type;

        if (type == Task.GetType())
            return;

        textBoxType.Text = treeViewTypes.SelectedNode.ToString();

        var oldTask = Task;
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

    private void buttonReload_Click(object sender, EventArgs e)
    {
        Init(Pipeline, OriginalTask);
    }

    private void treeViewTypes_BeforeSelect(object sender, TreeViewCancelEventArgs e)
    {
        e.Cancel = e.Node is not AssemblyTaskTypeTree.TypeInfo;
    }
}
