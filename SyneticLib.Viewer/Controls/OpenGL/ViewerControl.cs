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

using OpenTK.Mathematics;

using SyneticLib.Math3D;

namespace SyneticLib.WinForms.Controls.OpenGL;

public class ViewerControl : GLControl
{
    public static ViewerControl Instance { get; set; }

    DateTime _last;

    public GlScene Scene { get; private set; }
    public Camera Camera
    {
        get => Scene.Camera;
        set => Scene.Camera = value;
    }

    public ViewerControl()
    {
        _last = DateTime.Now;
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        if (DesignMode)
            return;



        base.OnHandleCreated(e);
    }

    protected override void OnLoad(EventArgs e)
    {
        if (DesignMode)
            return;

        base.OnLoad(e);

        API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
        APIVersion = new Version(4, 5, 0, 0);
        Flags = OpenTK.Windowing.Common.ContextFlags.Debug;

        Scene = new GlScene();
    }

    public void UpdateLogic()
    {
        var delta = DateTime.Now - _last;
        Camera.Update(delta);
        _last = DateTime.Now;
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);

        Focus();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        Camera.MouseMove(e.X, e.Y, e.Button == MouseButtons.Left);

        base.OnMouseMove(e);
    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
        Camera.Scroll(e.Delta);

        base.OnMouseWheel(e);
    }


    protected override void OnHandleDestroyed(EventArgs e)
    {
        if (DesignMode)
            return;

        Scene.Dispose();

        base.OnHandleDestroyed(e);
    }

    protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.W:
            case Keys.Up:
                {
                    Camera.Inputs.MoveUp = true;
                    break;
                }
            case Keys.A:
            case Keys.Left:
                {
                    Camera.Inputs.MoveLeft = true;
                    break;
                }
            case Keys.S:
            case Keys.Down:
                {
                    Camera.Inputs.MoveDown = true;
                    break;
                }
            case Keys.D:
            case Keys.Right:
                {
                    Camera.Inputs.MoveRight = true;
                    break;
                }
        }

        base.OnPreviewKeyDown(e);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.W:
            case Keys.Up:
                {
                    Camera.Inputs.MoveUp = false;
                    break;
                }
            case Keys.A:
            case Keys.Left:
                {
                    Camera.Inputs.MoveLeft = false;
                    break;
                }
            case Keys.S:
            case Keys.Down:
                {
                    Camera.Inputs.MoveDown = false;
                    break;
                }
            case Keys.D:
            case Keys.Right:
                {
                    Camera.Inputs.MoveRight = false;
                    break;
                }
        }

        base.OnKeyUp(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;

        if (DesignMode)
        {
            g.Clear(BackColor);
            return;
        }

        Render();

        g.Clear(Color.Red);
    }

    public void Render()
    {
        Camera.ScreenSize = new(ClientSize.Width, ClientSize.Height);
        Camera.CreatePerspective();
        Camera.CreateView();

        try
        {
            MakeCurrent();
        }
        catch (OpenTK.Windowing.GraphicsLibraryFramework.GLFWException)
        {
            return; // Context failed: drop frame
        }

        Scene.ClearScreen();
        Scene.Render();

        SwapBuffers();
    }
}
