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
    PipelineList Pipelines => PipelinesControl.Pipelines;

    public SynPipelineToolForm()
    {
        InitializeComponent();

        var executer = PipelinesControl.Executer;
        var piplines = PipelinesControl.Pipelines;

        PipelinesControl.TasksControl = TasksControl;

        piplines.TryLoad();

        PipelinesControl.InvalidateItems();
        if (piplines.Count > 0)
            PipelinesControl.SelectedItem = piplines[0];

        PipelinesControl.ListBox.Executer = executer;
        TasksControl.ListBox.Executer = executer;

    }

    private void quitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Pipelines.Clear();
        PipelinesControl.InvalidateItems();
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog();
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            Pipelines.Path = dialog.FileName;
            Pipelines.Clear();
            Catch(Pipelines.Load);
            PipelinesControl.InvalidateItems();
        }
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Catch(Pipelines.Save);
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        using var dialog = new SaveFileDialog();
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            Pipelines.Path = dialog.FileName;
            Catch(Pipelines.Save);
        }
    }

    private void Catch(Action action)
    {
        try
        {
            action();
        }
        catch (Exception e)
        {
            MessageBox.Show(this, e.Message, e.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
