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
    public readonly List<ModelInstance> Instances;
    public Terrain Terrain;
    public Camera Camera;

    public Texture GridTexture;
    public ModelMaterial GridMaterial;
    public Model Grid;
    public Model Skybox;

    public unsafe Scene()
    {
        Sprites = new List<Sprite>();
        Instances = new List<ModelInstance>();
        Camera = new OrbitCamera();

        int gridSize = 4_000_0;

        GridTexture = new Texture(2, 2);
        fixed (byte* bytes = GridTexture.PixelData)
        {
            uint* pixels = (uint*)bytes;

            for (int iy = 0; iy < GridTexture.Height; iy++)
            {
                for (int ix = 0; ix < GridTexture.Width; ix++)
                {
                    int i = (ix + iy * GridTexture.Width);

                    bool xodd = ix % 2 == 0;
                    bool yodd = iy % 2 == 0;

                    bool check = yodd ? xodd : !xodd;

                    pixels[i] = check ? (uint)0x969696 : (uint)0xA9A9A9;
                }
            }
        }
        GridTexture.DataState = DataState.Loaded;

        GridMaterial = new ModelMaterial();
        GridMaterial.TexSlot0.Enable(GridTexture);
        GridMaterial.DataState = DataState.Loaded;

        Grid = new Model()
        {
            Poligons = new Vector3Int[2]
            {
                new(2,1,0),
                new(0,3,2),
            },
            Vertices = new Vertex[4]
            {
                new(new(-gridSize,+gridSize,0),new(0,0)),
                new(new(+gridSize,+gridSize,0),new(10,0)),
                new(new(+gridSize,-gridSize,0),new(10,10)),
                new(new(-gridSize,-gridSize,0),new(0,10)),
            },
            MaterialRegion = new MaterialRegion[1]
            {
                new(0,2,GridMaterial),
            },
            DataState = DataState.Loaded
        };
    }


    public void ClearScreen() => Graphics.ClearScreen();

    public void ClearScene()
    {
        Terrain = null;
        Instances.Clear();
        Sprites.Clear();
    }

    private void DrawSprite(Sprite sprite)
    {
        float aspectTex = ((float)sprite.Texture.Width / (float)sprite.Texture.Height);
        float aspect = Camera.AspectRatio / aspectTex;
        float scale = 0.98f;

        if (aspect > 1.0f)
            scale *= 1.0f / aspect;



        Console.WriteLine(aspect);
        GL.Uniform2(Sprite.GLProgram.UScale, new Vector2(1 * scale, 1 * aspect * scale));


        Graphics.AssertRessource(sprite.Texture);

        if (!sprite.Texture.GLBuffer.TryCreate())
            throw new InvalidOperationException("Could not create GL buffer.");

        sprite.Texture.GLBuffer.Bind();

        GL.DrawElements(PrimitiveType.Triangles, Sprite.GLBuffer.ElementCount, DrawElementsType.UnsignedShort, 0 * 3 * 2);

    }

    public void Render()
    {
        Camera.CreatePerspective();
        Camera.CreateView();

        Graphics.BindCamera(Camera);

        GL.Viewport(0, 0, (int)Camera.ScreenSize.X, (int)Camera.ScreenSize.Y);
        GL.Disable(EnableCap.DepthTest);
        GL.Disable(EnableCap.CullFace);

        GridTexture.GLBuffer.TryCreate();
        GridTexture.GLBuffer.Bind();

        Graphics.DrawModel(Grid, Matrix4.Identity);

        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);

        if (Terrain != null)
            Graphics.DrawTerrain(Terrain);

        foreach (var instance in Instances)
            Graphics.DrawModel(instance);


        GL.Disable(EnableCap.DepthTest);

        if (!Sprite.GLBuffer.TryCreate())
            throw new InvalidOperationException("Could not create GL buffer.");

        if (!Sprite.GLProgram.TryCreate())
            throw new InvalidOperationException("Could not create GL buffer.");

        Sprite.GLBuffer.Bind();
        Sprite.GLProgram.Bind();

        foreach (Sprite sprite in Sprites)
            DrawSprite(sprite);
    }
}
