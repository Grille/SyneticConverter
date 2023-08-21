using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;
using System.Reflection;
using SyneticLib.LowLevel;
using SyneticLib.LowLevel.Files;
using SyneticLib.Locations;

using static System.IO.Path;

namespace SyneticLib.IO;
public static partial class Imports
{
    public static ScenarioGroup LoadScenarioGroup(string path, string name, GameVersion version)
    {
        var dirs = Directory.GetDirectories(path);

        var variants = new List<Scenario>();

        for (int i = 0; i < dirs.Length; i++)
        {
            var dirpath = dirs[i];
            string dirname = GetFileName(dirpath);
            if (dirname.Length == 2 && int.TryParse(dirname.Substring(1, 1), out int id))
            {
                var scenario = LoadScenario(dirpath, name, version);
                variants.Add(scenario);
            }
        }

        var group = new ScenarioGroup(name, variants.ToArray());
        return group;
    }


    public static Scenario LoadScenario(string path, string name, GameVersion version)
    {
        
        var syn = new SynFile();
        var geo = new GeoFile();
        var idx = new IdxFile();
        var lvl = new LvlFile();
        var sni = new SniFile();
        var vtx = new VtxFile();
        var qad = new QadFile();
        var sky = new SkyFile();

        IVertexData ivtx;
        IIndexData iidx;

        var synPath = Combine(path, $"V{0}");
        syn.Path = synPath + ".syn";

        var filePath = Combine(path, name);
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

        var terrainTextures = new TextureDirectory(Combine(path, "textures"));
        var modelTextures = new TextureDirectory(Combine(path, "objects/textures"));

        //target.Models.Load();

        // Materials
        var textureIndex = terrainTextures.CreateIndexedArray(qad.TextureNames);

        var materials = new List<Material>();

        for (var i = 0; i < qad.MaterialsWR.Length; i++)
        {
            var matInfo = qad.MaterialsWR[i];
            var mat = new Material($"Terrain_MAT_{i}");

            if (matInfo.Tex0Id < textureIndex.Length)
                mat.TexSlot0.Enable(textureIndex[matInfo.Tex0Id]);

            if (matInfo.Tex1Id < textureIndex.Length)
                mat.TexSlot1.Enable(textureIndex[matInfo.Tex1Id]);

            if (matInfo.Tex2Id < textureIndex.Length)
                mat.TexSlot2.Enable(textureIndex[matInfo.Tex2Id]);

            //mat.Mode = (TerrainMaterialType)(ushort)matInfo.Mode;
            //mat.DataState = DataState.Loaded;

            materials.Add(mat);
        }

        // Props
        /*
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

        

        var terrain = target.Terrain;
        terrain.Vertices = ivtx.Vertecis;
        terrain.Polygons = iidx.Indices;
        terrain.MaterialRegion = new ModelMaterialRegion[qad.PolyRegions.Length];
        for (var i = 0; i < qad.PolyRegions.Length; i++)
        {
            terrain.MaterialRegion[i] = new MaterialRegion
            (
                qad.PolyRegions[i].PolyOffset,
                qad.PolyRegions[i].PolyCount,
                target.TerrainMaterials[qad.PolyRegions[i].SurfaceId1]
            );
        }
        terrain.DataState = DataState.Loaded;

        /*
        for (int i = 0; i < qad.Chunks.Length; i++)
        {
            ref var chunkInfo = ref qad.Chunks[i];
            //var chunk = new ScenarioChunk(target, "");
            //var mesh = new Terrain(terrain);
            mesh.Vertices = vertices;
            //chunk.Terrain = terrain.CreateSectionPtr(chunkInfo.PolyRegionOffset, chunkInfo.PolyRegionCount);
            mesh.DataState = DataState.Loaded;
            terrain.Chunks.Add(mesh);
        }
        */

        // mesh data
        /*
        // Terrain mesh
        if (target.Terrain.Chunks.Count < 1)
            target.Terrain.Chunks.Add(new TerrainMesh(target.Terrain));

        var terrain = target.Terrain.Chunks[0];

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
        */

        // Chunks
        /*
        for (var i = 0; i < qad.Chunks.Length; i++)
        {
            ref var chunkInfo = ref qad.Chunks[i];
            var chunk = new ScenarioChunk(target, chunkInfo.PosX, chunkInfo.PosZ);
            //chunkInfo.
            chunk.TerrainMaterialRegionOffset = chunkInfo.PolyRegionOffset;
            chunk.TerrainMaterialRegionOffset = chunkInfo.PolyRegionCount;
            chunk.DataState = DataState.Loaded;
            target.Chunks.Add(chunk);
        }
        target.Chunks.DataState = DataState.Loaded;
        */

        return new Scenario(0);

    }
}
