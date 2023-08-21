using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Mathematics;
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

        var material = new Material("Checker");
        material.Diffuse = new Vector3(1, 1, 1);
        material.TexSlot0.Enable(Textures.Checker);

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

        var mesh = new Mesh("GroundPlane", vertices, indices);

        var plane = new Model("GroundPlane", mesh, material);

        return plane;
    }
}
