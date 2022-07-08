using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SyneticLib.IO.Synetic.Files;

namespace SyneticLib.IO;
public abstract class ScenarioImporter
{
    protected ScenarioVariant target;
    protected string path;

    public ScenarioImporter(ScenarioVariant target)
    {
        this.target = target;
        path = target.RootDir;
    }

    protected abstract void OnLoad();
    protected abstract void OnInit();

    public void Load()
    {
        target.State = ScenarioState.Loading;
        try
        {
            OnLoad();
            target.State = ScenarioState.Loaded;
        }
        catch (FileNotFoundException ex)
        {
            target.Errors.Add(ex.Message);
            target.State = ScenarioState.Failed;
        }
    }

    public void Init()
    {
        target.State = ScenarioState.Loading;
        OnInit();
        target.State = ScenarioState.Initialized;
    }

    public void Load(string path)
    {
        this.path = path;
        Load();
    }

    public void LoadAndInit()
    {
        Load();
        if (target.State == ScenarioState.Loaded)
            Init();
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

        mesh.MaterialRegion = new MaterialRegion[qad.MaterialRegions.Length];
        for (int i = 0; i < qad.MaterialRegions.Length; i++)
        {
            mesh.MaterialRegion[i] = new MaterialRegion()
            {
                Offset = qad.MaterialRegions[i].FirstPoly,
                Count = qad.MaterialRegions[i].NumPoly,
                Material = target.WorldMaterials[qad.MaterialRegions[i].SurfaceID],
            };
        }

        DeflateIndecies(idx, vtx, qad);

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

    protected void DeflateIndecies(IIndexData idx, IVertexData vtx, QadFile qad)
    {
        var terrain = target.Terrain;
        var mesh = terrain.Mesh;

        var srcidx = idx.Indices;
        var indecies = new int[srcidx.Length];
        var poligons = new Vector3Int[srcidx.Length / 3];

        int pos = 0;
        int offset = 0;
        int preXT = 0;
        for (int iz = 0; iz < qad.Head.BlocksZ; iz++)
        {
            for (int ix = 0; ix < qad.Head.BlocksX; ix++)
            {
                var block = qad.Blocks[iz, ix];
                if (block.Chunk65k != preXT)
                {
                    preXT = block.Chunk65k;
                    offset += vtx.VtxQty[block.Chunk65k - 1];
                }

                int begin = block.FirstPoly;
                int end = begin + block.NumPoly;
                for (int i = begin; i < end; i++)
                {
                    poligons[i].X = indecies[pos + 0] = srcidx[pos + 0] + offset;
                    poligons[i].Y = indecies[pos + 1] = srcidx[pos + 1] + offset;
                    poligons[i].Z = indecies[pos + 2] = srcidx[pos + 2] + offset;

                    pos += 3;
                }
            }
        }

        mesh.Indecies = indecies;
        mesh.Poligons = poligons;
    }
}
