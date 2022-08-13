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

    private void DrawTerrain(Terrain terrain)
    {
        if (terrain.GLBuffer.State != GLState.Ready)
            terrain.GLBuffer.Create();

        if (terrain.GLBuffer.State != GLState.Ready)
            return;
    }

    private void DrawMesh(MeshInstance instance)
    {
        if (instance.Mesh.GLBuffer.State != GLState.Ready)
            instance.Mesh.GLBuffer.Create();

        if (instance.Mesh.GLBuffer.State != GLState.Ready)
            return;
    }

    private void DrawSprite(Sprite sprite)
    {
        if (sprite.Texture.GLBuffer.State != GLState.Ready)
            sprite.Texture.GLBuffer.Create();

        if (sprite.Texture.GLBuffer.State != GLState.Ready)
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
