using SyneticPipelineTool.GUI;
using SyneticPipelineTool.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace SyneticPipelineTool;


public partial class SynPipelineToolForm : Form
{
    readonly AsyncPipelineExecuter Executer;
    readonly PipelineList Piplines;

    public Pipeline SelectedPipeline
    {
        get => pipelinesListBox.SelectedItem;
        set => pipelinesListBox.SelectedItem = value;
    }

    public PipelineTask SelectedTask
    {
        get => tasksListBox.SelectedItem;
        set => tasksListBox.SelectedItem = value;
    }

    public List<PipelineTask> SelectedTasks
    {
        get => tasksListBox.SelectedTasks;
        set => tasksListBox.SelectedTasks = value;
    }

    public SynPipelineToolForm()
    {
        InitializeComponent();
        Executer = new AsyncPipelineExecuter();
        Piplines = new PipelineList();
        Piplines.Load();

        PipelinesChanged();
        if (Piplines.Count > 0)
            SelectedPipeline = Piplines[0];

        refreshTimer.Interval = 500;

        pipelinesListBox.Executer = Executer;
        tasksListBox.Executer = Executer;
    }

    public void DisplayPipelines() => pipelinesListBox.UpdateItems(Piplines);

    public void DisplayTasks() => tasksListBox.UpdateItems(SelectedPipeline?.Tasks);

    public void PipelineSelectionChanged()
    {
        bool single = SelectedPipeline != null;

        buttonCopyP.Enabled = single;
        buttonEditP.Enabled = single;
        buttonRemoveP.Enabled = single;
        buttonExecuteP.Enabled = single;

        buttonUpP.Enabled = single;
        buttonDownP.Enabled = single;

        tasksListBox.Enabled = single;
        buttonNewT.Enabled = single;

        TasksChanged(false);
    }

    public void TaskSelectionChanged()
    {
        bool single = SelectedTasks.Count == 1;
        bool multi = SelectedTasks.Count > 1;
        bool invalid = SelectedTask is InvalidTypeTask;

        buttonCopyT.Enabled = single && !invalid;
        buttonEditT.Enabled = single;
        buttonRemoveT.Enabled = (single || multi);

        buttonUpT.Enabled = (single || multi);
        buttonDownT.Enabled = (single || multi);
    }

    public void PipelinesChanged(bool save = true)
    {
        DisplayPipelines();
        PipelineSelectionChanged();

        if (save)
            Piplines.Save();
    }

    public void TasksChanged(bool save = true)
    {
        DisplayTasks();
        TaskSelectionChanged();

        if (save)
            Piplines.Save();
    }

    private void pipelinesListBox_SelectedIndexChanged(object sender, EventArgs e) => PipelineSelectionChanged();

    private void tasksListBox_SelectedIndexChanged(object sender, EventArgs e) => TaskSelectionChanged();

    private void buttonExecuteP_Click(object sender, EventArgs e)
    {
        Executer.Execute(SelectedPipeline);
        PipelineSelectionChanged();
        TaskSelectionChanged();
    }

    private void buttonNewP_Click(object sender, EventArgs e)
    {
        var dialog = new TextBoxDialog();
        var result = dialog.ShowDialog(this, "New Pipeline", "Pipeline");
        if (result == DialogResult.OK)
        {
            string newname = dialog.TextResult.Trim();
            string uname = Piplines.GetUniqueName(newname);
            var pipeline = Piplines.CreateUnbound(uname);
            Piplines.InsertAfter(SelectedPipeline, pipeline);
            PipelinesChanged();
            SelectedPipeline = pipeline;
        }
    }

    private void buttonCopyP_Click(object sender, EventArgs e)
    {
        var clone = SelectedPipeline.Clone();
        Piplines.InsertAfter(SelectedPipeline, clone);
        PipelinesChanged();
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
            pipelinesListBox.Items.Add(0);
            PipelinesChanged();
        }
    }

    private void buttonRemoveP_Click(object sender, EventArgs e)
    {
        Piplines.Remove(pipelinesListBox.SelectedItem);
        PipelinesChanged();
    }

    private void buttonUpP_Click(object sender, EventArgs e)
    {
        var selected = SelectedPipeline;
        Piplines.UpItem(selected);
        PipelinesChanged();
    }

    private void buttonDownP_Click(object sender, EventArgs e)
    {
        var selected = SelectedPipeline;
        Piplines.DownItem(selected);
        PipelinesChanged();
    }

    private void buttonNewT_Click(object sender, EventArgs e)
    {
        var dialog = new EditTaksForm(SelectedPipeline);
        var result = dialog.ShowDialog(this);
        if (result == DialogResult.OK)
        {
            SelectedPipeline.Tasks.InsertAfter(SelectedTask, dialog.Task);
            TasksChanged();
            SelectedTask = dialog.Task;
            SelectedTask.Pipeline = SelectedPipeline;
        }
    }


    private void buttonEditT_Click(object sender, EventArgs e)
    {
        var dialog = new EditTaksForm(SelectedPipeline, SelectedTask);
        var result = dialog.ShowDialog(this);
        if (result == DialogResult.OK)
        {
            dialog.Task.Pipeline = SelectedPipeline;
            int idx = SelectedPipeline.Tasks.IndexOf(SelectedTask);
            SelectedPipeline.Tasks[idx] = dialog.Task;
            TasksChanged();
        }
    }

    private void buttonRemoveT_Click(object sender, EventArgs e)
    {
        foreach (var task in SelectedTasks)
        {
            SelectedPipeline.Tasks.Remove(task);
        }
        TasksChanged();
    }

    private void buttonCopyT_Click(object sender, EventArgs e)
    {
        var clone = SelectedTask.Clone();
        clone.Pipeline = SelectedPipeline;
        SelectedPipeline.Tasks.InsertAfter(SelectedTask, clone);
        TasksChanged();
    }

    private void buttonUpT_Click(object sender, EventArgs e)
    {
        var selected = SelectedTasks;
        foreach (var task in selected)
        {
            SelectedPipeline.Tasks.UpItem(task);
        }
        TasksChanged();
        SelectedTasks = selected;
    }

    private void buttonDownT_Click(object sender, EventArgs e)
    {
        var selected = SelectedTasks;
        selected.Reverse();
        foreach (var task in selected)
        {
            SelectedPipeline.Tasks.DownItem(task);
        }
        TasksChanged();
        SelectedTasks = selected;
    }

    private void tasksListBox_DoubleClick(object sender, EventArgs e)
    {
        if (SelectedTask == null || SelectedTask is InvalidTypeTask)
            return;
        buttonEditT_Click(sender, e);
    }

    private void pipelinesListBox_DoubleClick(object sender, EventArgs e)
    {
        if (SelectedPipeline == null)
            return;
        buttonEditP_Click(sender, e);
    }

    private void refreshTimer_Tick(object sender, EventArgs e)
    {
        if (Executer.Running)
        {
            pipelinesListBox.Invalidate();
            tasksListBox.Invalidate();
        }
    }


}
