using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticConverter;
public abstract class ScenarioImporter
{
    protected ScenarioVariant target;
    protected string path;

    public ScenarioImporter(ScenarioVariant target)
    {
        this.target = target;
        path = target.RootDir;
    }

    public abstract void Load();
    public abstract void Assign();

    public void Load(string path)
    {
        this.path = path;
        Load();
    }

    public void LoadAndAssign()
    {
        Load();
        Assign();
    }

    public void LoadWorldTexture(string name)
    {
        string path = Path.Combine(target.RootDir, "Textures", name);
        var texture = new Texture(name);
        texture.Id = target.WorldTextures.Count;
        texture.ImportPtx(path);
        target.WorldTextures.Add(texture);
    }

    public void LoadPropTexture(string name)
    {
        string path = Path.Combine(target.RootDir, "Objects/Textures", name);
        var texture = new Texture(name);
        texture.ImportPtx(path);
        target.PropTextures.Add(texture);
    }

    protected void AssignTerrain(IIndexData idx, IVertexData vtx, QadFile qad)
    {
        var terrain = target.Terrain;
        var mesh = terrain.Mesh;

        var vertices = mesh.Vertices = new Vertex[vtx.Vertices.Length];
        for (int i = 0; i < vtx.Vertices.Length; i++)
        {
            vertices[i] = vtx.Vertices[i];
        }

        mesh.Indecies = new int[idx.Indices.Length];
        for (int i = 0; i < idx.Indices.Length; i++)
        {
            mesh.Indecies[i] = idx.Indices[i];

            if (mesh.Indecies[i] > vertices.Length)
                throw new Exception();
        }



        mesh.PolyRegion = new PolyRegion[qad.Poly.Length];
        for (int i = 0; i < qad.Poly.Length; i++)
        {
            mesh.PolyRegion[i] = new PolyRegion()
            {
                Offset = qad.Poly[i].FirstPoly,
                Count = qad.Poly[i].NumPoly,
                Material = target.WorldMaterials[qad.Poly[i].SurfaceID],
            };
        }

        mesh.Poligons = GenerateTriangles(idx, vtx, qad);

        int maxc = 0;
        int max = mesh.Vertices.Length;

        for (int i = 0; i < mesh.Poligons.Length; i++)
        {
            ref var poly = ref mesh.Poligons[i];

            maxc = Math.Max(maxc, Math.Max(Math.Max(poly.X, poly.Y), poly.Z));

            //continue;

            if (poly.X < 0 || poly.Y < 0 || poly.Z < 0)
            {
                throw new Exception();
            }
            if (poly.X > max || poly.Y > max || poly.Z > max)
            {
                poly.X = 0; poly.Y = 0; poly.Z = 0;
                throw new Exception();
            }

        }
    }

    protected Vector3Int[] GenerateTriangles(IIndexData idx, IVertexData vtx, QadFile qad)
    {
        var indices = idx.Indices;
        var result = new Vector3Int[indices.Length / 3];


        int pos = 0;
        int xt = 0;
        int preXT = 0;
        for (int iz = 0; iz < qad.Head.BlocksZ; iz++)
        {
            for (int ix = 0; ix < qad.Head.BlocksX; ix++)
            {
                var block = qad.Blocks[iz, ix];
                if (block.Chunk65k != preXT)
                {
                    preXT = block.Chunk65k;
                    xt += vtx.VtxQty[block.Chunk65k - 1];
                }

                int begin = block.FirstPoly;
                int end = begin + block.NumPoly;
                for (int i = begin; i < end; i++)
                {
                    result[i].X = indices[pos + 0] + xt;
                    result[i].Y = indices[pos + 1] + xt;
                    result[i].Z = indices[pos + 2] + xt;

                    pos += 3;
                }
            }
        }

        return result;
    }
}
