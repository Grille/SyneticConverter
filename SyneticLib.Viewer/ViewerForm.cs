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

namespace SyneticLib.Viewer;

public partial class ViewerForm : Form
{
    Scene scene => viewerControl1.Scene;

    public ViewerForm()
    {
        InitializeComponent();
        DoubleBuffered = true;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        renderTimer.Start();
    }

    private void renderTimer_Tick(object sender, EventArgs e) => viewerControl1.Invalidate();

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var dialog = new OpenFileDialog();
        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            var path = dialog.FileName;
            var ext = Path.GetExtension(path);

            switch (ext)
            {
                case ".ptx":
                {
                    var texture = Imports.LoadPtxTexture(path);
                    break;
                }


                case ".mox":
                {
                    var model = Imports.LoadModelFromMox(path);

                    scene.Add(model);
                    break;
                }

                case ".idx":
                case ".vtx":
                case ".qad":
                case ".geo":
                {
                    var scenario = Imports.LoadScenario(Path.GetDirectoryName(path), "AUSTRALIEN", LowLevel.GameVersion.WR2);

                    scene.Add(scenario.Terrain);
                    break;
                }
            }
        }
    }
}
