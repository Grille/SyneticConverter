using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SyneticLib.Graphics;
using SyneticLib.IO;
using SyneticLib.Utils;
using SyneticLib.WinForms.Controls.OpenGL;

namespace SyneticLib.WinForms;

public class SceneLoader
{
    readonly Control _control;
    GlScene GLScene;

    public string FilePath { get; private set; }

    public string FileName => Path.GetFileName(FilePath);

    public SceneLoader(Control control, GlScene scene)
    {
        ArgumentNullException.ThrowIfNull(scene);

        _control = control;
        GLScene = scene;

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
        GLScene.ClearScene();
        GLScene.SubmitScenario(scenario);
    }

    public void LoadModelCob(string path)
    {
        var mesh = Serializers.Mesh.Cob.Load(path);
        GLScene.ClearScene();
        //scene.SubmitSingleModel(model);
    }

    public void LoadModel(string path)
    {
        var model = Serializers.Model.Synetic.Load(path);
        GLScene.ClearScene();
        GLScene.SubmitSingleModel(model);
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
        LoadFile();
    }

    public void LoadFile()
    {
        using var dialog = new OpenFileDialog();
        if (dialog.ShowDialog(_control) == DialogResult.OK)
        {
            var fileName = dialog.FileName;


            try
            {
                LoadFile(dialog.FileName);
            }
            catch (Exception ex)
            {

                ExceptionBox.Show(_control, ex);


            }
        }
    }

    public void LoadFile(string path)
    {
        FilePath = path;

        var ext = Path.GetExtension(path).ToLower();

        switch (ext)
        {
            case ".trk":
            {
                var track = Serializers.Track.Trk.Load(path);
                var texture = TrackMapGenerator.CreateTrackMap(track, 512, 512);
                GLScene.SubmitTexture(texture);
                break;
            }
            case ".tga":
            {
                var texture = Serializers.Texture.Tga.Load(path);
                GLScene.SubmitTexture(texture);
                break;
            }
            case ".ptx":
            {
                var texture = Serializers.Texture.Ptx.Load(path);
                GLScene.SubmitTexture(texture);
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
                dialog.Show(_control);
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
}
