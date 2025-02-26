using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

using SyneticLib.World;

namespace SyneticLib;
public class ModelBuilder
{
    public List<Triangle> Triangles { get; }

    public ModelBuilder()
    {
        Triangles = new List<Triangle>();
    }

    public void Add(ReadOnlySpan<Vertex> vertices, ReadOnlySpan<IdxTriangleInt32> triangles, int offset, Material material)
    {
        for (var i = 0; i < triangles.Length; i++)
        {
            var idx = triangles[i];
            var triangle = new Triangle()
            {
                X = vertices[idx.X + offset],
                Y = vertices[idx.Y + offset],
                Z = vertices[idx.Z + offset],
                Material = material,
            };
            Triangles.Add(triangle);
        }
    }

    public void Add(Model model)
    {
        var mesh = model.MeshSection;
        var vertices = mesh.Vertices;
        var triangles = mesh.Triangles;
        int offset = mesh.Offset;

        foreach (var mat in model.MaterialRegions)
        {
            var slice = triangles.Slice(mat.ElementStart, mat.ElementCount);
            Add(vertices, slice, offset, mat.Material);
        }
    }

    public void Add(Model model, Matrix4 matrix)
    {
        int start = Triangles.Count;

        Add(model);

        for (int i = start; i < Triangles.Count; i++)
        {
            Triangles[i].Apply(matrix);
        }
    }

    public void Add(TerrainModel terrain)
    {
        foreach (var model in terrain)
        {
            Add(model);
        }
    }

    public Model ToModel()
    {
        return null!;
    }

    /*
    public TerrainModel ToTerrainModel(Vector2 start, int width, int height, float size)
    {


        var m = new TerrainModel()
    }
    */

    public struct Triangle
    {
        public Vertex X, Y, Z;
        public Material Material;

        public Vector3 Center => (X.Position + Y.Position + Z.Position) / 3f;

        public void Apply(in Matrix4 matrix)
        {
            X.Position = (new Vector4(X.Position) * matrix).Xyz;
            Y.Position = (new Vector4(Y.Position) * matrix).Xyz;
            Z.Position = (new Vector4(Z.Position) * matrix).Xyz;
        }
    }
}
