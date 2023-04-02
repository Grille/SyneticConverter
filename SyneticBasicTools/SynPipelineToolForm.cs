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
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace SyneticPipelineTool;

public partial class SynPipelineToolForm : Form
{
    PipelineList Piplines;

    public Pipeline SelectedPipeline
    {
        get => (Pipeline)pipelinesListBox.SelectedItem;
        set => pipelinesListBox.SelectedItem = value;
    }

    public PipelineTask SelectedTask
    {
        get => (PipelineTask)tasksListBox.SelectedItem;
        set => tasksListBox.SelectedItem = value;
    }

    public SynPipelineToolForm()
    {
        InitializeComponent();
        Piplines = new PipelineList();
        Piplines.Load();
        PipelinesChanged();
        if (Piplines.Count > 0)
            SelectedPipeline = Piplines[0];

        tasksListBox.DrawMode = DrawMode.OwnerDrawFixed;
    }

    public void DisplayPipelines() => DisplayListBox(pipelinesListBox, Piplines);

    public void DisplayTasks(Pipeline pipeline) => DisplayListBox(tasksListBox, pipeline?.Tasks);

    public void DisplayListBox<T>(ListBox box, List<T> list) where T : class
    {
        var items = box.Items;

        if (list == null)
            return;

        box.BeginUpdate();

        var selectet = box.SelectedItem;

        if (items.Count == list.Count)
        {
            for (int i = 0; i< list.Count; i++)
            {
                if (items[i] != list[i])
                {
                    items[i] = list[i];
                }
            }
        }
        else
        {
            items.Clear();
            foreach (var item in list)
            {
                items.Add(item);
            }
            box.SelectedItem = selectet;
        }

        if (selectet != null && box.SelectedItem != selectet)
            box.SelectedItem = selectet;

        box.Invalidate();
        box.EndUpdate();
    }


    public void PipelineSelectionChanged()
    {
        bool pipelineFlag = SelectedPipeline != null;

        buttonCopyP.Enabled = pipelineFlag;
        buttonEditP.Enabled = pipelineFlag;
        buttonRemoveP.Enabled = pipelineFlag;
        buttonExecuteP.Enabled = pipelineFlag;

        buttonUpP.Enabled = pipelineFlag;
        buttonDownP.Enabled = pipelineFlag;

        tasksListBox.Enabled = pipelineFlag;
        buttonNewT.Enabled = pipelineFlag;

        TasksChanged(false);
    }

    public void TaskSelectionChanged()
    {
        bool taskFlag = SelectedTask != null;
        bool invalid = SelectedTask is InvalidTypeTask;

        buttonCopyT.Enabled = taskFlag & !invalid;
        buttonEditT.Enabled = taskFlag & !invalid;
        buttonRemoveT.Enabled = taskFlag;

        buttonUpT.Enabled = taskFlag;
        buttonDownT.Enabled = taskFlag;
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
        DisplayTasks(SelectedPipeline);
        TaskSelectionChanged();

        if (save)
            Piplines.Save();
    }

    private void pipelinesListBox_SelectedIndexChanged(object sender, EventArgs e) => PipelineSelectionChanged();

    private void tasksListBox_SelectedIndexChanged(object sender, EventArgs e) => TaskSelectionChanged();

    private void buttonExecuteP_Click(object sender, EventArgs e)
    {
        Console.WriteLine("Start");
        SelectedPipeline.Execute();
        Console.WriteLine("Stop");
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
        Piplines.Remove((Pipeline)pipelinesListBox.SelectedItem);
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
        SelectedPipeline.Tasks.Remove(SelectedTask);
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
        var selected = SelectedTask;
        SelectedPipeline.Tasks.UpItem(selected);
        TasksChanged();
        SelectedTask = selected;
    }

    private void buttonDownT_Click(object sender, EventArgs e)
    {
        var selected = SelectedTask;
        SelectedPipeline.Tasks.DownItem(selected);
        TasksChanged();
        SelectedTask = selected;
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

    private void tasksListBox_DrawItem(object sender, DrawItemEventArgs e)
    {
        var item = tasksListBox.Items[e.Index];

        e.DrawBackground();
        e.DrawFocusRectangle();
     
        var g = e.Graphics;

        SolidBrush brush;

        if (item is NopTask)
            brush = new SolidBrush(Color.Gray);
        else if (item is InvalidTypeTask)
            brush = new SolidBrush(Color.Red);
        else
            brush = new SolidBrush(e.ForeColor);

        g.DrawString(item.ToString(), e.Font, brush, (RectangleF)e.Bounds);
        
    }
}
