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

    public ScenarioImporter(ScenarioVariant target)
    {
        this.target = target;
    }

    protected abstract void OnLoad();
    protected abstract void OnInit();

    public void Load()
    {
        try
        {
            OnLoad();
            OnInit();
            target.DataState = DataState.Loaded;
        }
        catch (FileNotFoundException ex)
        {
            target.Errors.Add(ex.Message);
            target.DataState = DataState.Error;
        }
    }

    protected void AssignTerrain(IIndexData idx, IVertexData vtx, QadFile qad)
    {
        var terrain = target.Terrain;
        var mesh = terrain;

        var vertices = mesh.Vertices = new Vertex[vtx.Vertecis.Length];
        for (int i = 0; i < vtx.Vertecis.Length; i++)
        {
            vertices[i] = vtx.Vertecis[i];
        }

        mesh.MaterialRegion = new MaterialRegion[qad.MaterialRegions.Length];
        for (int i = 0; i < qad.MaterialRegions.Length; i++)
        {
            mesh.MaterialRegion[i] = new MaterialRegion
            (
                qad.MaterialRegions[i].FirstPoly,
                qad.MaterialRegions[i].NumPoly,
                target.TerrainMaterials[qad.MaterialRegions[i].SurfaceID]
            );
        }

        DeflateIndecies(idx, vtx, qad);
        /*
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
        */

        terrain.DataState = DataState.Loaded;
    }

    protected void DeflateIndecies(IIndexData idx, IVertexData vtx, QadFile qad)
    {
        var terrain = target.Terrain;
        var mesh = terrain;

        var srcidx = idx.Polygons;
        var dstidx = new Vector3Int[srcidx.Length];

        int pos = 0;
        int offset = 0;
        int preXT = 0;

        terrain.Chunks = new TerrainChunk[qad.Head.BlocksTotal];

        for (int iz = 0; iz < qad.Head.BlocksZ; iz++)
        {
            for (int ix = 0; ix < qad.Head.BlocksX; ix++)
            {
                int index = ix + iz * qad.Head.BlocksX;

                ref var srcchunk = ref qad.Blocks[index];

                terrain.Chunks[index] = new TerrainChunk()
                {
                    ElementOffset = srcchunk.FirstPoly,
                    ElementCount = srcchunk.NumPoly,
                };

                var block = qad.Blocks[index];
                if (block.Chunk65k != preXT)
                {
                    preXT = block.Chunk65k;
                    offset += vtx.VtxQty[block.Chunk65k - 1];
                }

                int begin = block.FirstPoly;
                int end = begin + block.NumPoly;
                for (int i = begin; i < end; i++)
                {
                    dstidx[pos].X = srcidx[pos].X + offset;
                    dstidx[pos].Y = srcidx[pos].Y + offset;
                    dstidx[pos].Z = srcidx[pos].Z + offset;

                    pos += 1;
                }
            }
        }

        mesh.Poligons = dstidx;
    }
}
