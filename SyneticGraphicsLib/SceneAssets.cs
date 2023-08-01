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

        byte[] pixels = new byte[2 * 2 * 4];
        fixed (byte* bptr = pixels)
        {
            uint* iptr = (uint*)bptr;
            iptr[0] = iptr[3] = 0x969696;
            iptr[1] = iptr[2] = 0xA9A9A9;
        }

        var texture = new Texture("", TextureFormat.RGBA32, 2, 2, pixels);

        var material = new Material("");
        material.TexSlot0.Enable(texture);

        var indices = new IndexTriangle[2]
        {
            new(2, 1, 0),
            new(0, 3, 2),
        };

        var vertices = new Vertex[4]
        {
            new(new(-gridSize, +gridSize, 0), new(0, 0)),
            new(new(+gridSize, +gridSize, 0), new(uvScale, 0)),
            new(new(+gridSize, -gridSize, 0), new(uvScale, uvScale)),
            new(new(-gridSize, -gridSize, 0), new(0, uvScale)),
        };

        var mesh = new Mesh("", vertices, indices);

        var plane = new Model("", mesh, material);

        return plane;
    }
}
