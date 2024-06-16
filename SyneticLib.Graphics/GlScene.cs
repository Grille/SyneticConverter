using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;
using OpenTK.Compute.OpenCL;
using SyneticLib.Graphics.OpenGL;
using SyneticLib.Graphics.DrawCalls;
using SyneticLib.Math3D;
using System.Diagnostics;

namespace SyneticLib.Graphics;
public class GlScene : IDisposable
{
    public SpriteBatch Sprites { get; }

    public Camera Camera;

    SceneAssets assets;

    Vector2 textureSize;
    TextureBuffer? textureBuffer;
    ModelGlHandle? modelHandle;
    ScenarioGlHandle? scenarioHandle;

    GlObjectCacheGroup cache;



    public unsafe GlScene()
    {
        Sprites = new SpriteBatch();
        Camera = new FreeCamera();

        assets = new SceneAssets();

        cache = new GlObjectCacheGroup();
    }

    public void SubmitTexture(Texture texture)
    {
        textureBuffer?.Dispose();
        textureBuffer = new TextureBuffer(texture);
        var size = texture.Size;
        textureSize = size / MathF.Max(size.X, size.Y);
    }

    public void SubmitScenario(Scenario scenario)
    {
        scenarioHandle?.Dispose();
        scenarioHandle = new ScenarioGlHandle(scenario);
    }

    public void SubmitSingleModel(Model model)
    {
        modelHandle?.Dispose();
        modelHandle = new ModelGlHandle(model);
    }

    public void ClearScreen()
    {
        GL.ClearColor(Color.AliceBlue);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    public void ClearScene()
    {
        textureBuffer?.Dispose();
        textureBuffer = null;

        scenarioHandle?.Dispose();
        scenarioHandle = null;

        modelHandle?.Dispose();
        modelHandle = null;

        Sprites.Clear();
    }

    public void Render()
    {
        GL.Viewport(0, 0, (int)Camera.ScreenSize.X, (int)Camera.ScreenSize.Y);
        GL.Enable(EnableCap.CullFace);
        GL.CullFace(CullFaceMode.Front);

        var plane = assets.GroundPlane;
        plane.SubCamera(Camera);

        /*
        foreach (Sprite sprite in Sprites)
        {
            var texture = GlobalCache.Create(sprite.Texture);
            texture.Bind();
        }
        */
        GL.Disable(EnableCap.DepthTest);
        {
            plane.DrawModel(Matrix4.Identity);
        }

        GL.Enable(EnableCap.DepthTest);

        var sw = new Stopwatch();

        if (scenarioHandle != null)
        {
            scenarioHandle.SubCamera(Camera);
            scenarioHandle.DrawScenario(Camera.Position, 32);
        }

        if (modelHandle != null)
        {
            modelHandle.SubCamera(Camera);
            modelHandle.DrawModel();
        }

        /*
        //GL.Disable(EnableCap.DepthTest);

        {
            var closest = RayCaster.NoHit;

            var ray = Camera.CastMouseRay();

            if (scenario != null)
            {
                foreach (var chunk in scenario.EnumerateChunks())
                {
                    closest.ApplyIfCloserHit(RayCaster.RayIntersectsModel(ray, chunk.Terrain));
                }
            }

            if (modelHandle != null)
            {
                closest.ApplyIfCloserHit(RayCaster.RayIntersectsModel(ray, model!));
            }

            if (!closest.IsHit)
            {
                closest = RayCaster.RayIntersectsGround(ray);
            }



            var pos = closest.GetIntersection(ray);

            var mat = Matrix4.CreateTranslation(pos);


            assets.Compass.DrawModel(mat);
        }
        */
        //GL.DrawElements(PrimitiveType.Triangles, buffer.ElementCount, DrawElementsType.UnsignedInt, 0 * 3 * 4);

        var combinedSize = Camera.ScreenSize * textureSize.Yx;
        var normalizedScreenSize = (combinedSize / MathF.Max(combinedSize.X, combinedSize.Y)).Yx;


        if (textureBuffer != null)
        {
            var sprite = new Sprite(textureBuffer, new Vector2(0, 0), normalizedScreenSize);
            Sprites.Clear();
            Sprites.Add(sprite);
            Sprites.Draw();
        }


        var error = GL.GetError();
        if (error != ErrorCode.NoError)
        {
            Console.WriteLine($"{DateTime.Now} {error}");
        }

        //Console.WriteLine(sw.ElapsedMilliseconds);


        //GL.Enable(EnableCap.DepthTest | EnableCap.CullFace);


        /*
        foreach (var instance in MeshDrawBuffer)
            Graphics.DrawMesh(instance);

        Graphics.DepthTestEnabled = false;
        */

    }


    public void Dispose()
    {
        Sprites.Dispose();

        assets.Dispose();
        cache.Dispose();
        textureBuffer?.Dispose();
        modelHandle?.Dispose();
        scenarioHandle?.Dispose();
    }
}
