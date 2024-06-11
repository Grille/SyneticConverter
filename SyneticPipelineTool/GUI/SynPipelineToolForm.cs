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

        Icon = Icon.FromHandle(Icons.SyneticLib.GetHicon());
        MBWRToolStripMenuItem.Image = Icons.MBWR;
        WR2ToolStripMenuItem.Image = Icons.WR2;
        C11ToolStripMenuItem.Image = Icons.C11;
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

    private void MBWRToolStripMenuItem_Click(object sender, EventArgs e)
    {
        ShowPreview(GameVersion.WR1);
    }

    private void WR2ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        ShowPreview(GameVersion.WR2);
    }

    private void C11ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        ShowPreview(GameVersion.C11);
    }

    void ShowPreview(GameVersion version)
    {
        var preview = new ViewerForm();
        preview.Show();
        preview.ShowLoadScenarioDialog(version);
    }
}
