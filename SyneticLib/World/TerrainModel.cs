using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

namespace SyneticLib.World;
public class TerrainModel : IReadOnlyCollection<Model>
{
    public int Width { get; }
    public int Height { get; }
    public int Length { get; }

    public IndexedMesh Mesh { get; }

    public int Count => _chunks.Length;

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
        var vtx = Mesh.Vertices.ToArray();
        var idx = Mesh.Triangles.ToArray();
        var mat = new List<ModelMaterialRegion>();

        for (int ix = 0; ix < Width; ix++)
        {
            for (int iz = 0; iz < Height; iz++)
            {
                var src = _chunks[ix, iz];

                for (int i = 0; i < src.MeshSection.Length; i++)
                {
                    idx[src.MeshSection.Start + i] += src.MeshSection.Offset;
                }

                foreach (var srcmat in src.MaterialRegions)
                {
                    mat.Add(new ModelMaterialRegion(srcmat.ElementStart, srcmat.ElementCount, srcmat.Material));
                }
            }
        }

        var mesh = new IndexedMesh(vtx.ToArray(), idx.ToArray());
        var section = new MeshSegment(mesh);
        return new Model(section, mat.ToArray());
    }

    public IEnumerator<Model> GetEnumerator()
    {
        for (int ix = 0; ix < Width; ix++)
        {
            for (int iz = 0; iz < Height; iz++)
            {
                yield return _chunks[ix, iz];
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
