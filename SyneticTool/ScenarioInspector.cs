using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SyneticConverter;
using OpenTK.WinForms;
using OpenTK.Graphics.OpenGL4;

namespace SyneticTool
{
    public partial class ScenarioInspector : Form
    {
        Scenario scenario;
        Camera camera;
        Renderer renderer;
        GLControl gLControl;

        public ScenarioInspector(Scenario scn)
        {
            InitializeComponent();

            DoubleBuffered = true;

            scenario = scn;

            MouseWheel += ScenarioInspector_MouseWheel;

            camera = new();
            renderer = new();

            camera.Scale = 100;
            renderTimer.Interval = 20;
            renderTimer.Start();

            gLControl = new GLControl();
            gLControl.BackColor = Color.Black;
            gLControl.Dock = DockStyle.Fill;
            Controls.Add(gLControl);

            gLControl.Resize += MyGLControl_Resize;
            gLControl.Paint += MyGLControl_Paint;


        }

        private void ScenarioInspector_MouseWheel(object sender, MouseEventArgs e)
        {
            camera.MouseScrollEvent(e, 1.5f);
        }

        private void ScenarioInspector_Shown(object sender, EventArgs e)
        {
            camera.ScreenSize = ClientSize;
            scenario.LoadAllData();
        }

        private void ScenarioInspector_Paint(object sender, PaintEventArgs e)
        {
            //renderer.Render(e.Graphics, camera, scenario);
        }

        private void ScenarioInspector_Resize(object sender, EventArgs e)
        {
            camera.ScreenSize = ClientSize;
        }

        private void ScenarioInspector_MouseMove(object sender, MouseEventArgs e)
        {
            camera.MouseMoveEvent(e, e.Button.HasFlag(MouseButtons.Left));
        }

        private void renderTimer_Tick(object sender, EventArgs e)
        {
            Render();
        }


        private void MyGLControl_Resize(object? sender, EventArgs e)
        {
            gLControl.MakeCurrent();    // Tell OpenGL to use MyGLControl.

            // Update OpenGL on the new size of the control.
            GL.Viewport(0, 0, gLControl.ClientSize.Width, gLControl.ClientSize.Height);

            /*
                Usually you compute projection matrices here too, like this:

                float aspect_ratio = MyGLControl.ClientSize.Width / (float)MyGLControl.ClientSize.Height;
                Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 64);

                And then you load that into OpenGL with a call like GL.LoadMatrix() or GL.Uniform().
            */
        }

        private void MyGLControl_Paint(object? sender, PaintEventArgs e)
        {
            Render();
        }

        public void Render()
        {
            gLControl.MakeCurrent();    // Tell OpenGL to draw on MyGLControl.
            GL.ClearColor(Color.DarkBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit);                // Clear any prior drawing.

            /*
            ... use various other GL.*() calls here to draw stuff ...
            */

            gLControl.SwapBuffers();    // Display the result.
        }
    }
}
