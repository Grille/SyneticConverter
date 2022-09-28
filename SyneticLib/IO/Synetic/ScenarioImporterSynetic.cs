using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;
using SyneticLib.IO.Synetic.Files;
using static SyneticLib.IO.Synetic.Files.QadFile;
using System.Reflection;

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

    public ScenarioImporterSynetic(Scenario target) : base(target)
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
    }

    public void LoadV(int iv)
    {
        if (iv > Target.Variants.Count)
            return;
        OnLoadV(Target.Variants[iv-1]);
    }

    protected override void OnLoad()
    {
        foreach (var v in Target.Variants)
        {
            OnLoadV(v);
        }
    }


    protected void OnLoadV(ScenarioVariant target)
    {
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

        target.Sounds.Load();
        target.TerrainTextures.Load();
        target.ModelTextures.Load();
        //target.Models.Load();

        // Apply textures
        var textureIndex = target.TerrainTextures.CreateIndexedArray(qad.TextureNames);

        for (var i = 0; i < qad.Materials.Length; i++)
        {
            var matInfo = qad.Materials[i];
            var mat = new TerrainMaterial(Target);

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

        // Props
        for (var i = 0; i < qad.Head.PropClassCount; i++)
        {
            var name = qad.PropClassObjNames[i];
            var info = qad.PropClassInfo[i];
            var prop = new PropClass(target, name);
            prop.DataState = DataState.Loaded;
            target.PropClasses.Add(prop);
        }

        for (var i = 0; i < qad.Head.PropInstanceCount; i++)
        {
            var propIntanceInfo = qad.PropInstances[i];
            var propClass = target.PropClasses[propIntanceInfo.ClassId];
            target.PropInstances.Add(new PropInstance(Target, propClass)
            {
                Class = propClass,
                Position = propIntanceInfo.Position,
            });
        }
        

        target.PropClasses.DataState = DataState.Loaded;

        // Lights
        foreach (var lightInfo in qad.Lights)
        {
            var light = new Light(Target);

            light.Color = lightInfo.Color;
            light.Position = lightInfo.Matrix.Translation;

            light.DataState = DataState.Loaded;
            target.Lights.Add(light);
        }

        target.Lights.DataState = DataState.Loaded;

        // Terrain
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


        void DeflateTerrainIndecies(IIndexData idx, IVertexData vtx, QadFile qad)
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
    }
}
