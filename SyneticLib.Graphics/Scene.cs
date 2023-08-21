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
    public readonly List<DrawCallInfo> MeshDrawBuffer;
    public Camera Camera;

    public Context Context { get; }

    readonly SceneAssets assets;

    MeshBuffer buffer;
    MaterialProgram program;
    TextureBuffer texture;

    public unsafe Scene()
    {
        Sprites = new List<Sprite>();
        MeshDrawBuffer = new List<DrawCallInfo>();
        Camera = new OrbitCamera();
        assets = new SceneAssets();

        Context = new Context();

        buffer = Context.Create(assets.GroundPlane.Mesh);
        program = Context.Create(assets.GroundPlane.MaterialRegions[0].Material);
        texture = Context.Create(assets.GroundPlane.MaterialRegions[0].Material.TexSlot0.Texture);
    }

    public void Add(Model mesh) => Add(mesh, Matrix4.Identity);

    public void Add(Model model, in Matrix4 matrix)
    {
        buffer = Context.Create(model.Mesh);
        program = Context.Create(model.MaterialRegions[0].Material);
        //mesh.MaterialRegion
    }

    public void ClearScreen()
    {
        GL.ClearColor(Color.AliceBlue);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    public void ClearScene()
    {
        MeshDrawBuffer.Clear();
        Sprites.Clear();
    }

    public void Render()
    {
        GL.Viewport(0, 0, (int)Camera.ScreenSize.X, (int)Camera.ScreenSize.Y);
        GL.Enable(EnableCap.CullFace);
        GL.Enable(EnableCap.DepthTest);

        //texture.Bind();
        buffer.Bind();
        program.Bind();
        program.TextureBinding0.Texture.Bind();

        program.SubCameraMatrix(Camera);
        program.SubModelMatrix(Matrix4.Identity);

        foreach (Sprite sprite in Sprites)
        {
            var texture = Context.Create(sprite.Texture);
            texture.Bind();
        }

        var reg = new MeshBufferRegionInfo(buffer, 0, buffer.ElementCount);
        reg.DrawElements();
        //GL.DrawElements(PrimitiveType.Triangles, buffer.ElementCount, DrawElementsType.UnsignedInt, 0 * 3 * 4);

        var error = GL.GetError();
        if (error != ErrorCode.NoError)
        {
            Console.WriteLine($"{DateTime.Now} {error}");
        }
 



        //GL.Enable(EnableCap.DepthTest | EnableCap.CullFace);


        /*
        foreach (var instance in MeshDrawBuffer)
            Graphics.DrawMesh(instance);

        Graphics.DepthTestEnabled = false;
        */
     
    }
}
