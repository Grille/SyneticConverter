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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Diagnostics;
using SyneticLib.WinForms.Resources;
using SyneticLib.Utils;

namespace SyneticLib.WinForms;

public partial class ViewerForm : Form
{
    static ViewerForm? _globalInstance;
    public static ViewerForm GlobalInstance
    {
        get
        {
            if (_globalInstance == null)
            {
                _globalInstance = new ViewerForm(true);
            }
            return _globalInstance;
        }
    }

    GlScene scene => viewerControl1.Scene;

    public bool HideOnCLose { get; }

    public ViewerForm(bool hideOnCLose = false)
    {
        HideOnCLose = hideOnCLose;

        InitializeComponent();
        DoubleBuffered = true;
        Icon = EmbeddedImageList.SyneticLib.Icon;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        renderTimer.Start();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (HideOnCLose && e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
            scene.ClearScene();
            Hide();
        }
        base.OnFormClosing(e);
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
        var name = Path.GetFileNameWithoutExtension(path);
        var scenario = Serializers.Scenario.Synetic.Load(Path.GetDirectoryName(path)!, name);
        scene.ClearScene();
        scene.SubmitScenario(scenario);
    }

    public void LoadModelCob(string path)
    {
        var mesh = Serializers.Mesh.Cob.Load(path);
        scene.ClearScene();
        //scene.SubmitSingleModel(model);
    }

    public void LoadModel(string path)
    {
        var model = Serializers.Model.Synetic.Load(path);
        scene.ClearScene();
        scene.SubmitSingleModel(model);
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
        LoadFile();
    }

    public void LoadFile()
    {
        using var dialog = new OpenFileDialog();
        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            var fileName = dialog.FileName;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    LoadFile(dialog.FileName);
                }
                catch (Exception ex)
                {
                    Invoke(() =>
                    {
                        ExceptionBox.Show(this, ex);
                    });
                }
            });
        }
    }

    public void LoadFile(string path)
    {
        var ext = Path.GetExtension(path).ToLower();

        switch (ext)
        {
            case ".trk":
            {
                var track = Serializers.Track.Trk.Load(path);
                var texture =  TrackMapGenerator.CreateTrackMap(track, 512, 512);
                scene.SubmitTexture(texture);
                break;
            }
            case ".tga":
            {
                var texture = Serializers.Texture.Tga.Load(path);
                scene.SubmitTexture(texture);
                break;
            }
            case ".ptx":
            {
                var texture = Serializers.Texture.Ptx.Load(path);
                scene.SubmitTexture(texture);
                break;
            }
            case ".cob":
            {
                LoadModelCob(path);
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
                using var dialog = new ProgressDialog();
                dialog.Show(this);
                LoadScenario(path);
                dialog.Close();
                break;
            }
            default:
            {
                throw new FileFormatException($"File of type {ext} are not supported.");
            }
        }
    }

    private void clearToolStripMenuItem_Click(object sender, EventArgs e)
    {
        scene.ClearScene();
    }

    private void asObjToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var dialog = new ExportSceneAsObjDialog();
    }
}
