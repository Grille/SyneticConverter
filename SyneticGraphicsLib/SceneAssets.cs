using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.LowLevel;

namespace SyneticLib.Graphics;

internal class SceneAssets
{
    public Model GroundPlane { get; }

    public unsafe SceneAssets()
    {
        GroundPlane = CreateGroundPlane(8);
    }

    public unsafe static Model CreateGroundPlane(int size)
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

        var mesh = new Mesh()
        {
            Indices = new IndexTriangle[2]
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
        };

        var plane = new Model()
        {
            Polygons = new IndexTriangle[2]
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
            MaterialRegion = new ModelMaterialRegion[1]
            {
                new(0, 2, material),
            },
            DataState = DataState.Loaded
        };

        return plane;
    }
}
