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

        //target.Sounds.Load();
        target.TerrainTextures.Load();
        target.ModelTextures.Load();
        //target.Models.Load();

        // Materials
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

        // Terrain mesh
        var terrain = target.Terrain;

        var vertices = terrain.Vertices = new Vertex[ivtx.Vertecis.Length];
        for (int i = 0; i < ivtx.Vertecis.Length; i++)
        {
            vertices[i] = ivtx.Vertecis[i];
        }

        terrain.MaterialRegion = new MaterialRegion[qad.PolyRegions.Length];
        for (int i = 0; i < qad.PolyRegions.Length; i++)
        {
            terrain.MaterialRegion[i] = new MaterialRegion
            (
                qad.PolyRegions[i].PolyOffset,
                qad.PolyRegions[i].PolyCount,
                target.TerrainMaterials[qad.PolyRegions[i].SurfaceId1]
            );
        }

        // Inflate terrain indecies 16bit to 32bit
        terrain.Poligons = new Vector3Int[idx.Polygons.Length];
        int idxpos = 0;
        int idxoffset = 0;
        for (int i = 0; i < vtx.VtxQty.Length; i++)
        {
            int count = vtx.VtxQty[i];
            idxoffset += count;

            for (int i2 = 0; i2 < count; i2++)
            {
                terrain.Poligons[idxpos] = idx.Polygons[idxpos] + idxoffset;
                idxpos++;
            }
        }
        terrain.DataState = DataState.Loaded;

        // Chunks
        for (int i = 0; i< qad.Chunks.Length;i++)
        {
            ref var chunkInfo = ref qad.Chunks[i];
            var chunk = new ScenarioChunk(target, "");
            chunk.Terrain = terrain.CreateSectionPtr(chunkInfo.PolyRegionOffset, chunkInfo.PolyRegionCount);
            chunk.DataState = DataState.Loaded;
            target.Chunks.Add(chunk);
        }
        target.Chunks.DataState = DataState.Loaded;
    }
}
