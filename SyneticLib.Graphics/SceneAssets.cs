using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Mathematics;
using System.Threading.Tasks;
using SyneticLib.Graphics.OpenGL;
using SyneticLib.Graphics.DrawCalls;

namespace SyneticLib.Graphics;

public class SceneAssets : IDisposable
{
    public ModelGlHandle GroundPlane { get; }

    public ModelGlHandle Compass { get; }

    public MeshBuffer SpriteMesh { get; }

    //public static MeshBuffer MeshBuffer { get; }

    GlObjectCacheGroup _cache;

    public SceneAssets()
    {
        _cache = new();
        //var error = CreateChecker("Error", 0xFFFF44FF, 0xFF444444);

        var compass = CreateCompas();
        var plane = CreateGroundPlane(8,8, new Vector3(0.8f, 1, 1));
        GroundPlane = new ModelGlHandle(plane, _cache);
        Compass = new ModelGlHandle(compass, _cache);

        _cache.Uncouple();
    }

    public ShaderProgram Shader { get; }

    static unsafe Texture CreateChecker(string name, uint color1, uint color2)
    {
        byte[] pixels = new byte[2 * 2 * 4];
        fixed (byte* bptr = pixels)
        {
            uint* iptr = (uint*)bptr;
            iptr[0] = iptr[3] = color1;
            iptr[1] = iptr[2] = color2;
        }
        return new Texture(name, TextureFormat.RGBA32, 2, 2, pixels);
    }

    public static Model CreateCompas()
    {
        return CreateGroundPlane(0.001f,1, new Vector3(1f, 1, 1));
    }

    public static Model CreateGroundPlane(float size, float uvScale, Vector3 color)
    {
        float gridSize = size * 1000_0f;

        var checker = CreateChecker("Checker", 0xFF969696, 0xFFA9A9A9);

        var material = new Material();
        material.ShaderType = MaterialShaderType.Water;
        material.Diffuse = color;
        material.TexSlot0.Enable(checker);

        var indices = new IdxTriangleInt32[2]
        {
            new(0, 1, 2),
            new(2, 3, 0),
        };

        var vertices = new Vertex[4]
        {
            new(new(-gridSize, 0, +gridSize), new(0, 0)),
            new(new(+gridSize, 0, +gridSize), new(uvScale, 0)),
            new(new(+gridSize, 0, -gridSize), new(uvScale, uvScale)),
            new(new(-gridSize, 0, -gridSize), new(0, uvScale)),
        };

        var mesh = new Mesh(vertices, indices);

        var submesh = new MeshSegment(mesh);

        var plane = new Model(submesh, material);

        return plane;
    }

    public void Dispose()
    {
        _cache.Dispose();
    }
}
