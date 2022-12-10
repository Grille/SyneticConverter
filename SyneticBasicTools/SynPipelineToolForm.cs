using SyneticBasicTools.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticBasicTools;

public partial class SynPipelineToolForm : Form
{
    PiplineList Piplines;

    public Pipeline SelectedPipeline
    {
        get => (Pipeline)pipelinesListBox.SelectedItem;
        set => pipelinesListBox.SelectedItem = value;
    }

    public SynPipelineToolForm()
    {
        InitializeComponent();
        Piplines = new PiplineList();
        Piplines.TryLoad();
        DisplayPipelines();
        if (pipelinesListBox.Items.Count > 0)
            pipelinesListBox.SelectedIndex = 0;

        PipelineChanged();
    }

    public void DisplayPipelines()
    {
        pipelinesListBox.Items.Clear();

        pipelinesListBox.BeginUpdate();
        foreach (var pipeline in Piplines)
        {
            pipelinesListBox.Items.Add(pipeline);
        }
        pipelinesListBox.EndUpdate();
    }

    public void DisplayTasks(Pipeline pipeline)
    {
        tasksListBox.Items.Clear();

        if (pipeline == null)
            return;

        tasksListBox.BeginUpdate();
        foreach (var task in pipeline.Tasks)
        {
            tasksListBox.Items.Add(task);
        }
        tasksListBox.EndUpdate();
    }


    public void PipelineChanged()
    {
        var selected = SelectedPipeline;
        bool enable = selected != null;

        tasksListBox.Enabled = enable;
        buttonEditP.Enabled = enable;
        buttonRemoveP.Enabled = enable;
        buttonExecuteP.Enabled = enable;

        DisplayTasks(selected);

        Piplines.Save();
    }

    private void pipelinesListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        PipelineChanged();
    }

    private void buttonNewP_Click(object sender, EventArgs e)
    {
        var dialog = new TextBoxDialog();
        var result = dialog.ShowDialog(this, "New Pipeline", "Pipeline");
        if (result == DialogResult.OK)
        {
            string newname = dialog.TextResult.Trim();
            string uname = Piplines.GetUniqueName(newname);
            var pipeline = Piplines.Create(uname);

            DisplayPipelines();
            pipelinesListBox.SelectedItem = pipeline;
        }
    }


    private void buttonCopyP_Click(object sender, EventArgs e)
    {
        var clone = SelectedPipeline.Clone();
        DisplayPipelines();
        pipelinesListBox.SelectedItem = clone;
    }

    private void buttonEditP_Click(object sender, EventArgs e)
    {
        string name = SelectedPipeline.Name;
        var dialog = new TextBoxDialog();
        var result = dialog.ShowDialog(this, "New Pipeline", name);
        if (result == DialogResult.OK)
        {
            string newname = dialog.TextResult.Trim();
            if (newname == name)
                return;

            string uname = Piplines.GetUniqueName(newname);
            Piplines.Rename(name, uname);
            DisplayPipelines();
        }
    }

    private void buttonRemoveP_Click(object sender, EventArgs e)
    {
        Piplines.Remove((Pipeline)pipelinesListBox.SelectedItem);
        DisplayPipelines();
        PipelineChanged();
    }

    private void buttonNewT_Click(object sender, EventArgs e)
    {
        var dialog = new EditTaksForm(SelectedPipeline);
        var result = dialog.ShowDialog(this);
        if (result == DialogResult.OK)
        {
            SelectedPipeline.Tasks.Add(dialog.Task);
        }
    }
}
