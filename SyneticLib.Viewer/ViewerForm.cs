using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.WinForms;
using SyneticLib.Locations;
using SyneticLib.IO;
using SyneticLib.Graphics;

using SyneticLib.Resources;

namespace SyneticLib.Viewer;

public partial class ViewerForm : Form
{
    Scene scene => viewerControl1.Scene;

    public ViewerForm()
    {
        InitializeComponent();
        DoubleBuffered = true;
        Icon = Icon.FromHandle(Icons.SyneticLib.GetHicon());
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        renderTimer.Start();
    }

    private void renderTimer_Tick(object sender, EventArgs e)
    {
        viewerControl1.UpdateLogic();
        viewerControl1.Invalidate();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Close();
    }

    public void ShowLoadScenarioDialog(GameVersion version)
    {
        using var dialog = new OpenFileDialog();
        dialog.Filter = "Supported Files|*.qad;*.idx;*.vtx;*.geo;*.lvl|QAD Files|*.qad|IDX Files|*.idx|VTX Files|*.vtx|GEO Files|*.geo|LVL Files|*.lvl";
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            LoadScenario(dialog.FileName, version);
        }
    }

    public void LoadScenario(string path)
    {
        LoadScenario(path, GameVersion.WR2);
    }

    public void LoadScenario(string path, GameVersion version)
    {
        try
        {
            var name = Path.GetFileNameWithoutExtension(path);
            var scenario = Imports.LoadScenario(Path.GetDirectoryName(path), name);
            scene.ClearScene();
            scene.SubmitScenario(scenario);
        }
        catch (Exception ex)
        {
            ExceptionBox.Show(this, ex);
        }
    }

    public void LoadModel(string path)
    {
        try
        {
            var model = Imports.LoadModelFromMox(path);
            scene.ClearScene();
            scene.SubmitSingleModel(model);
        }
        catch (Exception ex)
        {
            ExceptionBox.Show(this, ex);
        }
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var dialog = new OpenFileDialog();
        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            var path = dialog.FileName;
            var ext = Path.GetExtension(path).ToLower();

            switch (ext)
            {
                case ".ptx":
                {
                    var texture = Imports.LoadPtxTexture(path);
                    break;
                }
                case ".mox":
                {
                    LoadModel(path);
                    break;
                }
                case ".lvl":
                case ".sni":
                case ".idx":
                case ".vtx":
                case ".qad":
                case ".geo":
                {
                    LoadScenario(path);
                    break;
                }
                default:
                {
                    ExceptionBox.Show(this, $"File of type {ext} are not supported.", "Unsupported file type");
                    break;
                }
            }
        }
    }
}
