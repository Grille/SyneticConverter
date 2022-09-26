using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;
using SyneticLib.IO.Synetic.Files;

namespace SyneticLib.IO.Synetic;
public class ScenarioImporterSynetic : ScenarioImporter
{
    private GameVersion version;

    private SynFile syn;
    private GeoFile geo;
    private IdxFile idx;
    private LvlFile lvl;
    private SniFile sni;
    private VtxFile vtx;
    private QadFile qad;
    private SkyFile sky;

    private IVertexData ivtx;
    private IIndexData iidx;

    public ScenarioImporterSynetic(ScenarioVariant target) : base(target)
    {
        version = target.Version;

        syn = new();
        geo = new();
        idx = new();
        lvl = new();
        sni = new();
        vtx = new();
        qad = new();
        sky = new();

        var synPath = Path.Combine(target.SourcePath, $"V{target.IdNumber}");
        syn.Path = synPath + ".syn";

        var filePath = Path.Combine(target.SourcePath, target.Parent.FileName);
        geo.Path = filePath + ".geo";
        idx.Path = filePath + ".idx";
        lvl.Path = filePath + ".lvl";
        sni.Path = filePath + ".sni";
        vtx.Path = filePath + ".vtx";
        qad.Path = filePath + ".qad";
        sky.Path = filePath + ".sky";

        qad.SetFlagsAccordingToVersion(version);
        geo.SetFlagsAccordingToVersion(version);
    }

    protected override void OnLoad()
    {
        if (version >= GameVersion.C11)
        {
            geo.Load();
            iidx = geo;
            ivtx = geo;
        }
        else
        {
            idx.Load();
            vtx.Load();
            iidx = idx;
            ivtx = vtx;
        }

        lvl.Load();
        sni.Load();
        qad.Load();

        if (sky.Exists)
            sky.Load();
    }

    protected override void OnInit()
    {
        AssignTextures();
        AssignObjects();
        AssignTerrain();
        AssignLights();
    }

    public void AssignTextures()
    {
        var textureIndex = CreateTextureIndex();

        for (var i = 0; i < qad.Materials.Length; i++)
        {
            var matInfo = qad.Materials[i];
            var mat = new TerrainMaterial(target);

            if (matInfo.Tex0Id < textureIndex.Length)
                mat.TexSlot0.Enable(textureIndex[matInfo.Tex0Id]);

            if (matInfo.Tex1Id < textureIndex.Length)
                mat.TexSlot1.Enable(textureIndex[matInfo.Tex1Id]);

            if (matInfo.Tex2Id < textureIndex.Length)
                mat.TexSlot2.Enable(textureIndex[matInfo.Tex2Id]);

            mat.Mode = (TerrainMaterialType)matInfo.Mode;
            mat.DataState = DataState.Loaded;

            target.TerrainMaterials.Add(mat);
        }
    }

    private void AssignObjects()
    {
        for (var i = 0; i < qad.Head.PropClassCount; i++)
        {
            var name = qad.PropObjNames[i];
            var data = qad.PropClasses[i];

            var prop = new PropClass(target, name);
            var path = Path.Combine(target.SourcePath, "Objects", name + ".mox");

            prop.DataState = DataState.Loaded;

            target.PropClasses.Add(prop);
        }


        for (var i = 0; i < qad.Head.PropInstanceCount; i++)
        {
            var propIntanceInfo = qad.PropInstances[i];
            Console.WriteLine(propIntanceInfo.ClassId);
            var propClass = target.PropClasses[propIntanceInfo.ClassId];
            target.PropInstances.Add(new PropInstance(target,propClass)
            {
                Class = propClass,
                Position = propIntanceInfo.Position,
            });
        }

        target.PropClasses.DataState = DataState.Loaded;

    }

    private void AssignLights()
    {
        foreach (var lightInfo in qad.Lights)
        {
            var light = new Light(target);

            light.Color = lightInfo.Color;
            light.Position = lightInfo.Matrix.Translation;

            light.DataState = DataState.Loaded;
            target.Lights.Add(light);
        }

        target.Lights.DataState = DataState.Loaded;
    }

    protected void AssignTerrain()
    {
        var terrain = target.Terrain;
        var mesh = terrain;

        var vertices = mesh.Vertices = new Vertex[ivtx.Vertecis.Length];
        for (int i = 0; i < ivtx.Vertecis.Length; i++)
        {
            vertices[i] = ivtx.Vertecis[i];
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

        DeflateTerrainIndecies(iidx, ivtx, qad);

        terrain.DataState = DataState.Loaded;
    }

    protected void DeflateTerrainIndecies(IIndexData idx, IVertexData vtx, QadFile qad)
    {
        var terrain = target.Terrain;
        var mesh = terrain;

        var srcidx = idx.Polygons;
        var dstidx = new Vector3Int[srcidx.Length];

        int pos = 0;
        int offset = 0;
        int preXT = 0;

        terrain.Chunks = new TerrainChunkInfo[qad.Head.BlocksTotal];

        for (int iz = 0; iz < qad.Head.BlocksZ; iz++)
        {
            for (int ix = 0; ix < qad.Head.BlocksX; ix++)
            {
                int index = ix + iz * qad.Head.BlocksX;

                ref var srcchunk = ref qad.Blocks[index];

                terrain.Chunks[index] = new TerrainChunkInfo()
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

    public Texture[] CreateTextureIndex()
    {
        var result = new Texture[qad.TextureNames.Length];
        for (int i = 0; i < qad.TextureNames.Length; i++)
        {
            result[i] = target.TerrainTextures.GetByFileName(qad.TextureNames[i]);
        }
        return result;
    }
}
