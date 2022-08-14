using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;

namespace SyneticLib.Graphics;
public class Scene
{
    public readonly List<Sprite> Sprites;
    public readonly List<MeshInstance> Instances;
    public Terrain Terrain;
    public Camera Camera;

    public Scene()
    {
        Sprites = new List<Sprite>();
        Instances = new List<MeshInstance>();
        Camera = new Camera();

        Camera.Position = new Vector3(0, 10000, -20000);
        Camera.LookAt(new Vector3(0, 0, -10000));
    }


    public void ClearScreen()
    {
        GL.ClearColor(Color.Blue);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    public void ClearScene()
    {
        Instances.Clear();
        Sprites.Clear();
    }

    private void AssertRessource(Ressource ressource)
    {
        if (ressource.DataState != DataState.Loaded)
            throw new ArgumentException("ressource", "Ressource is not loaded.");
    }

    private void DrawTerrain(Terrain terrain)
    {
        AssertRessource(terrain);

        if (!terrain.GLBuffer.TryCreate())
            return;
    }

    private void DrawMesh(MeshInstance instance)
    {
        AssertRessource(instance.Mesh);

        if (!instance.Mesh.GLBuffer.TryCreate())
            return;
    }

    private void DrawSprite(Sprite sprite)
    {
        AssertRessource(sprite.Texture);

        if (!sprite.Texture.GLBuffer.TryCreate())
            return;
    }

    public void Render()
    {
        GL.Viewport(0, 0, (int)Camera.ScreenSize.X, (int)Camera.ScreenSize.Y);
        GL.Enable(EnableCap.DepthTest);

        if (Terrain != null)
            DrawTerrain(Terrain);

        foreach (var instance in Instances)
            DrawMesh(instance);

        GL.Disable(EnableCap.DepthTest);

        foreach (Sprite sprite in Sprites)
            DrawSprite(sprite);
    }
}
