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
    public readonly List<MeshDrawCommand> MeshDrawBuffer;
    public Camera Camera;
    public Mesh GroundPlane;

    public unsafe Scene()
    {
        Sprites = new List<Sprite>();
        MeshDrawBuffer = new List<MeshDrawCommand>();
        Camera = new OrbitCamera();
        GroundPlane = CreateGroundPlane(8);
    }

    public unsafe static Mesh CreateGroundPlane(int size)
    {
        int gridSize = size * 1000_0;
        int uvScale = size;

        var texture = new Texture(2, 2);
        fixed (byte* bytes = texture.PixelData)
        {
            uint* pixels = (uint*)bytes;
            pixels[0] = pixels[3] = 0x969696;
            pixels[1] = pixels[2] = 0xA9A9A9;
        }
        texture.DataState = DataState.Loaded;

        var material = new ModelMaterial();
        material.TexSlot0.Enable(texture);
        material.DataState = DataState.Loaded;

        var plane = new Model()
        {
            Poligons = new Vector3Int[2]
            {
                new(2, 1, 0),
                new(0, 3, 2),
            },
            Vertices = new Vertex[4]
            {
                new(new(-gridSize, +gridSize, 0), new(0, 0)),
                new(new(+gridSize, +gridSize, 0), new(uvScale, 0)),
                new(new(+gridSize, -gridSize, 0), new(uvScale, uvScale)),
                new(new(-gridSize, -gridSize, 0), new(0, uvScale)),
            },
            MaterialRegion = new MaterialRegion[1]
            {
                new(0, 2, material),
            },
            DataState = DataState.Loaded
        };

        return plane;
    }

    public Mesh Raycast(Vector2 position)
    {
        return null;
    }

    public void ClearScreen() => Graphics.ClearScreen();

    public void ClearScene()
    {
        MeshDrawBuffer.Clear();
        Sprites.Clear();
    }

    public void Render()
    {
        Graphics.BindCamera(Camera);
        Graphics.SubCameraScreenSize();

        Graphics.DepthTestEnabled = false;
        Graphics.CullFaceEnabled = false;

        Graphics.DrawMesh(GroundPlane);

        Graphics.DepthTestEnabled = true;
        Graphics.CullFaceEnabled = true;

        foreach (var instance in MeshDrawBuffer)
            Graphics.DrawMesh(instance);

        Graphics.DepthTestEnabled = false;

        foreach (Sprite sprite in Sprites)
            Graphics.DrawSprite(sprite);
    }
}
