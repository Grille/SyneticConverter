using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;
using OpenTK.Compute.OpenCL;

namespace SyneticLib.Graphics;
public class Scene
{
    public readonly List<Sprite> Sprites;
    public readonly List<DrawCall> MeshDrawBuffer;
    public Camera Camera;

    public GLObjectRegistry<Texture, TextureBuffer> TextureRegistry { get; }
    public GLObjectRegistry<Model, ModelBuffer> ModelRegistry { get; }


    readonly SceneAssets assets;

    GLMeshBuffer buffer;
    GLProgram program;
    TextureBuffer texture;

    public unsafe Scene()
    {
        Sprites = new List<Sprite>();
        MeshDrawBuffer = new List<DrawCall>();
        Camera = new OrbitCamera();
        assets = new SceneAssets();

        TextureRegistry = new(tex => new(tex));
        ModelRegistry = new(model => new(model));

        buffer = new ModelBuffer(assets.GroundPlane);
        program = new ModelProgram(assets.GroundPlane.MaterialRegions[0].Material);
        texture = new TextureBuffer(assets.GroundPlane.MaterialRegions[0].Material.TexSlot0.Texture);
    }

    public Mesh Raycast(Vector2 position)
    {
        return null;
    }

    public void Add(Model mesh) => Add(mesh, Matrix4.Identity);

    public void Add(Model mesh, in Matrix4 matrix)
    {
        buffer = new ModelBuffer(assets.GroundPlane);
        program = new ModelProgram(assets.GroundPlane.MaterialRegions[0].Material);
        //mesh.MaterialRegion
    }

    public void ClearScreen()
    {
        GL.ClearColor(Color.AliceBlue);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.ColorBufferBit);
    }

    public void ClearScene()
    {
        MeshDrawBuffer.Clear();
        Sprites.Clear();
    }

    public void Render()
    {
        GL.Viewport(0, 0, (int)Camera.ScreenSize.X, (int)Camera.ScreenSize.Y);

        //GL.Disable(EnableCap.DepthTest | EnableCap.CullFace);

        texture.Bind();
        buffer.Bind();
        program.Bind();

        program.SubCameraMatrix(Camera);
        program.SubModelMatrix(Matrix4.Identity);

        GL.DrawElements(PrimitiveType.Triangles, 2 * 3, DrawElementsType.UnsignedInt, 0 * 3 * 4);

        Console.WriteLine(GL.GetError());



        //GL.Enable(EnableCap.DepthTest | EnableCap.CullFace);


        /*
        foreach (var instance in MeshDrawBuffer)
            Graphics.DrawMesh(instance);

        Graphics.DepthTestEnabled = false;

        foreach (Sprite sprite in Sprites)
            Graphics.DrawSprite(sprite);
        */
    }
}
