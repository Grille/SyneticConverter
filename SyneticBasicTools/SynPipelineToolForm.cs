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
    AsyncPipelineExecuter executer;
    PipelineList Piplines;
    bool timerCleanup = false;

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

    public List<PipelineTask> SelectedTasks
    {
        get
        {
            var list = new List<PipelineTask>();
            foreach (var item in tasksListBox.SelectedItems)
            {
                list.Add((PipelineTask)item);
            }
            return list;
        }
        set
        {
            tasksListBox.ClearSelected();
            foreach (var task in value)
            {
                int idx = tasksListBox.Items.IndexOf(task);
                tasksListBox.SetSelected(idx, true);
            }
        }
    }

    public SynPipelineToolForm()
    {
        InitializeComponent();
        executer = new AsyncPipelineExecuter();
        Piplines = new PipelineList();
        Piplines.Load();
        PipelinesChanged();
        if (Piplines.Count > 0)
            SelectedPipeline = Piplines[0];

        refreshTimer.Interval = 100;

        pipelinesListBox.DrawMode= DrawMode.OwnerDrawFixed;
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
        buttonEditT.Enabled = single && !invalid;
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
        DisplayTasks(SelectedPipeline);
        TaskSelectionChanged();

        if (save)
            Piplines.Save();
    }

    private void pipelinesListBox_SelectedIndexChanged(object sender, EventArgs e) => PipelineSelectionChanged();

    private void tasksListBox_SelectedIndexChanged(object sender, EventArgs e) => TaskSelectionChanged();

    private void buttonExecuteP_Click(object sender, EventArgs e)
    {
        executer.Execute(SelectedPipeline);
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

    private void tasksListBox_DrawItem(object sender, DrawItemEventArgs e)
    {
        if (e.Index == -1)
            return;

        var pipeline = SelectedPipeline;
        var task = tasksListBox.Items[e.Index];

        e.DrawBackground();

        var g = e.Graphics;

        Brush brushLineBack = Brushes.Gainsboro;
        Brush brushLine = Brushes.DimGray;
        Brush brushText;

        if (task is NopTask)
            brushText = new SolidBrush(Color.Gray);
        else if (task is InvalidTypeTask)
            brushText = new SolidBrush(Color.Red);
        else
            brushText = new SolidBrush(Color.Black);

        if (e.State.HasFlag(DrawItemState.Selected))
        {
            //brushLine = Brushes.White;
            brushText = Brushes.White;
        }

        if (executer.CallStack.Contains(pipeline) && pipeline.TaskPosition == e.Index) {
            brushLineBack = Brushes.LightGreen;
            brushLine = Brushes.DarkGreen;
        }

        int lineColumnWidth = 24;

        var boundsLine = (RectangleF)e.Bounds;
        boundsLine.Width = lineColumnWidth;
        var boundsText = (RectangleF)e.Bounds;
        boundsText.Width -= lineColumnWidth;
        boundsText.X += lineColumnWidth;

        g.FillRectangle(brushLineBack, boundsLine);

        g.DrawString((e.Index + 1).ToString(), e.Font, brushLine, boundsLine);

        g.DrawString(task.ToString(), e.Font, brushText, boundsText);

    }

    private void pipelinesListBox_DrawItem(object sender, DrawItemEventArgs e)
    {
        if (e.Index == -1)
            return;

        var pipeline = Piplines[e.Index];

        e.DrawBackground();

        var g = e.Graphics;

        Brush brushLineBack = Brushes.Gainsboro;
        Brush brushLine = Brushes.DimGray;
        Brush brushText;

        brushText = new SolidBrush(Color.Black);

        if (e.State.HasFlag(DrawItemState.Selected))
        {
            //brushLine = Brushes.White;
            brushText = Brushes.White;
        }

        bool stackContains = executer.CallStack.Contains(pipeline);

        if (stackContains)
        {
            brushLineBack = Brushes.LightGreen;
            brushLine = Brushes.DarkGreen;
        }

        int lineColumnWidth = 24;

        var boundsLine = (RectangleF)e.Bounds;
        boundsLine.Width = lineColumnWidth;
        var boundsText = (RectangleF)e.Bounds;
        boundsText.Width -= lineColumnWidth;
        boundsText.X += lineColumnWidth;

        g.FillRectangle(brushLineBack, boundsLine);

        if (stackContains)
        {
            var list = executer.CallStack.ToList();
            list.Reverse();
            int stackIdx = list.IndexOf(pipeline);
            g.DrawString((stackIdx + 1).ToString(), e.Font, brushLine, boundsLine);
        }

        g.DrawString(pipeline.ToString(), e.Font, brushText, boundsText);
    }

    private void refreshTimer_Tick(object sender, EventArgs e)
    {
        if (executer.Running)
        {
            pipelinesListBox.Invalidate();
            tasksListBox.Invalidate();
            timerCleanup = true;
        }
        else if (timerCleanup)
        {
            pipelinesListBox.Invalidate();
            tasksListBox.Invalidate();
            timerCleanup = false;
        }
    }


}
