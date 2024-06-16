using SyneticPipelineTool.GUI;
using SyneticPipelineTool.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

using Grille.PipelineTool;
using Grille.PipelineTool.WinForms;
using Grille.PipelineTool.IO;
using SyneticLib.Resources;
using SyneticLib.Viewer;
using SyneticLib;

namespace SyneticPipelineTool;


public partial class SynPipelineToolForm : Form
{
    private PipelinesControl PipelinesControl => PipelineToolControl.PipelinesControl;
    private PipelineTasksControl TasksControl => PipelineToolControl.TasksControl;
    PipelineList Pipelines => PipelinesControl.Pipelines;

    public SynPipelineToolForm()
    {
        InitializeComponent();

        if (File.Exists(PipelineToolControl.FilePath))
            PipelineToolControl.LoadFile();

        var piplines = PipelinesControl.Pipelines;
        PipelinesControl.InvalidateItems();
        if (piplines.Count > 0)
            PipelinesControl.SelectedItem = piplines[0];

        PipelineToolControl.ItemsChanged += PipelineToolControl_ItemsChanged;

        PipelineToolControl.UnsavedChangesChanged += PipelineToolControl_UnsavedChangesChanged;

        Icon = Icon.FromHandle(Icons.SyneticLib.GetHicon());
    }

    private void PipelineToolControl_UnsavedChangesChanged(object sender, EventArgs e)
    {
        const string title = "Synetic Pipeline Tool";
        Text = PipelineToolControl.UnsavedChanges ? $"{title} (*unsaved)" : title; 
    }

    private void PipelineToolControl_ItemsChanged(object sender, EventArgs e)
    {
        PipelineToolControl.SaveFile("autosave.txt");
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
        PipelineToolControl.ShowOpenFileDialog();
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
        PipelineToolControl.SaveFile();
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        PipelineToolControl.ShowSaveFileDialog();
    }

    private void SynPipelineToolForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (PipelineToolControl.ShowSaveExitDialog() == DialogResult.Cancel)
        {
            e.Cancel = true;
        }
    }

    private void previewToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var preview = ViewerForm.GlobalInstance;
        preview.Show();
        preview.LoadFile();
    }
}
