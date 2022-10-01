using OpenTK.Mathematics;
using OpenTK.WinForms;
using SyneticLib;
using SyneticLib.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static OpenTK.Graphics.OpenGL.GL;

namespace SyneticTool;

public class Display
{
    public readonly GLControl GLControl;
    public readonly Scene Scene;
    public Camera Camera
    {
        get => Scene.Camera;
        set => Scene.Camera = value;
    }

    public Display(GLControl gLControl)
    {
        GLControl = gLControl;
        Scene = new Scene();
    }

    public void ShowCar(Car car)
    {
        Scene.ClearScene();
        Scene.MeshDrawBuffer.Add(car.Model);
    }

    public void ShowMesh(Mesh mesh)
    {
        Scene.ClearScene();
        Scene.MeshDrawBuffer.Add(mesh);
    }

    public void ShowScenario(Scenario scenario)
    {
        ShowScenario(scenario.V1);
    }

    public void ShowScenario(ScenarioVariant v)
    {
        Scene.ClearScene();
        Scene.MeshDrawBuffer.Add(v.Terrain);
        /*
        foreach (var instance in v.PropInstances)
        {
            Scene.Meshes.Add((MeshInstance)instance);
        }
        */
    }

    public void ShowTexture(Texture texture)
    {
        Scene.ClearScene();
        Scene.Sprites.Add(new Sprite(texture));
    }

    public void Render()
    {
        Camera.ScreenSize = new(GLControl.ClientSize.Width, GLControl.ClientSize.Height);
        Camera.CreatePerspective();
        Camera.CreateView();

        try
        {
            GLControl.MakeCurrent();
        }
        catch (OpenTK.Windowing.GraphicsLibraryFramework.GLFWException)
        {
            return; // Context failed: drop frame
        }

        Scene.ClearScreen();
        Scene.Render();

        GLControl.SwapBuffers();
    }
}

