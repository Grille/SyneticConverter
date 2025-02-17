using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

namespace SyneticLib.World;
public class TerrainModel
{
    public int Width { get; }
    public int Height { get; }
    public int Length { get; }

    public IndexedMesh Mesh { get; }

    readonly Model[,] _chunks;

    public TerrainModel(IndexedMesh mesh, Model[,] chunks)
    {
        Width = chunks.GetLength(0);
        Height = chunks.GetLength(1);
        Length = chunks.Length;
        Mesh = mesh;
        _chunks = chunks;
        FillChunks(chunks);
    }

    void FillChunks(Model[,] chunks)
    {
        for (var iy = 0; iy < Height; iy++)
        {
            for (var ix = 0; ix < Width; ix++)
            {
                var chunk = _chunks[ix, iy];
                if (chunk.MeshSection.Mesh != Mesh)
                {
                    throw new ArgumentException();
                }
                _chunks[ix, iy] = chunks[ix, iy];
            }
        }
    }

    public static TerrainModel CreateTerrainFromModel(Model model)
    {
        var boundings = model.BoundingBox;

        var size = boundings.Size;
        var offset = boundings.Start;

        return null;
    }

    public Model GetModel(int x, int y)
    {
        return _chunks[x, y];
    }

    public Model ToModel()
    {
        var vtx = new List<Vertex>();
        var idx = new List<IdxTriangleInt32>();
        var mat = new List<ModelMaterialRegion>();

        for (int ix = 0; ix < Width; ix++)
        {
            for (int iz = 0; iz < Height; iz++)
            {
                var src = _chunks[ix, iz];

                foreach (var srcmat in src.MaterialRegions)
                {
                    mat.Add(new ModelMaterialRegion(0, 0, srcmat.Material));
                }
            }
        }

        var mesh = new IndexedMesh(vtx.ToArray(), idx.ToArray());
        var section = new MeshSegment(mesh);
        return new Model(section, mat.ToArray());
    }
}
