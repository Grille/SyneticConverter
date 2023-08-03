using OpenTK.WinForms;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SyneticLib.Graphics;

namespace SyneticLib.Viewer;

public partial class ViewerControl : UserControl
{
    GLControl glControl;

    public Scene Scene { get; private set; }
    public Camera Camera
    {
        get => Scene.Camera;
        set => Scene.Camera = value;
    }

    public ViewerControl()
    {
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (DesignMode)
            return;

        glControl = new GLControl(new()
        {
            API = OpenTK.Windowing.Common.ContextAPI.OpenGL,
            APIVersion = new Version(4, 5, 0, 0),
            Flags = OpenTK.Windowing.Common.ContextFlags.Debug,
        });

        glControl.BackColor = Color.Black;
        glControl.Dock = DockStyle.Fill;
        MouseWheel += GlPanel_MouseWheel;
        glControl.MouseMove += GlPanel_MouseMove;
        Controls.Add(glControl);

        Scene = new Scene();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (DesignMode)
            return;

        Render();
    }

    private void GlPanel_MouseMove(object sender, MouseEventArgs e)
    {
        Camera.MouseMove(e.X, e.Y, e.Button == MouseButtons.Left);
    }

    private void GlPanel_MouseWheel(object sender, MouseEventArgs e)
    {
        Camera.Scroll(e.Delta);
    }

    public void Render()
    {
        Camera.ScreenSize = new(glControl.ClientSize.Width, glControl.ClientSize.Height);
        Camera.CreatePerspective();
        Camera.CreateView();

        try
        {
            glControl.MakeCurrent();
        }
        catch (OpenTK.Windowing.GraphicsLibraryFramework.GLFWException)
        {
            return; // Context failed: drop frame
        }

        Scene.ClearScreen();
        Scene.Render();

        glControl.SwapBuffers();
    }
}
