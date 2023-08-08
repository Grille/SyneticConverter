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

            if (ext == ".ptx")
            {
                var texture = Imports.LoadPtxTexture(path);
                var sprite = new Sprite(texture);

                scene.ClearScene();
                scene.Sprites.Add(sprite);
            }
        }
    }
}
