using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;
using System.Reflection;
using SyneticLib.Files;
using SyneticLib.Locations;

using static System.IO.Path;
using System.Security.Cryptography.X509Certificates;

namespace SyneticLib.IO;
public static partial class Imports
{
    public static ScenarioGroup LoadScenarioGroup(string path, string name)
    {
        var dirs = Directory.GetDirectories(path);

        var variants = new List<Scenario>();

        for (int i = 0; i < dirs.Length; i++)
        {
            var dirpath = dirs[i];
            string dirname = GetFileName(dirpath);
            if (dirname.Length == 2 && int.TryParse(dirname.AsSpan(1, 1), out int id))
            {
                var scenario = LoadScenario(dirpath, name);
                variants.Add(scenario);
            }
        }

        var group = new ScenarioGroup(name, variants.ToArray());
        return group;
    }

    public static Scenario LoadScenario(string filePath)
    {
        var dirPath = GetDirectoryName(filePath);
        var fileName = GetFileNameWithoutExtension(filePath);
        return LoadScenario(dirPath!, fileName);
    }

    public static Scenario LoadScenario(string dirPath, string fileName)
    {
        var files = new ScenarioFiles();

        files.Deserialize(dirPath, fileName);

        var syn = files.Syn;
        var geo = files.Geo;
        var idx = files.Idx;
        var lvl = files.Lvl;
        var sni = files.Sni;
        var vtx = files.Vtx;
        var qad = files.Qad;
        var sky = files.Sky;

        var ivtx = files.VertexData;
        var iidx = files.IndexData;

        var terrainTextures = new TextureDirectory(Combine(dirPath, "textures"));
        var modelTextures = new TextureDirectory(Combine(dirPath, "objects/textures"));
        var terrainTextureIndex = terrainTextures.CreateIndexedArray(qad.TextureNames);

        var terrainMaterials = GetTerrainMaterials(qad, terrainTextureIndex);

        var offsets = new int[ivtx.IndicesOffset.Length + 1];
        for (var i = 0; i < offsets.Length - 1; i++)
        {
            offsets[i + 1] = offsets[i] + ivtx.IndicesOffset[i];
        }

        var matx = new Material();
        var mesh = new Mesh(ivtx.Vertecis, iidx.Indices);

        var scenario = new Scenario(0);

        scenario.Width = qad.Head.BlockCountX;
        scenario.Height = qad.Head.BlockCountZ;

        scenario.Chunks = new ScenarioChunk[qad.Head.BlockCountX, qad.Head.BlockCountZ];


        scenario.Terrain = mesh;

        for (int i = 0; i< qad.Chunks.Length; i++)
        {
            ref var srcChunk = ref qad.Chunks[i];

            var chunk = new ScenarioChunk(srcChunk.PosX, srcChunk.PosZ);
            int offset = offsets[srcChunk.Chunk65k];

            var submesh = new MeshSegment(mesh, srcChunk.PolyOffset, srcChunk.PolyCount, offset);

            
            var regions = new ModelMaterialRegion[srcChunk.PolyRegionCount];
            for (int iy = 0; iy< regions.Length; iy++)
            {
                var srcRegion = qad.PolyRegions[iy + srcChunk.PolyRegionOffset];
                var region = new ModelMaterialRegion(srcRegion.PolyOffset, srcRegion.PolyCount, terrainMaterials[srcRegion.SurfaceId1]);
                regions[iy] = region;
            }
         

            var model = new Model(submesh, regions);

            chunk.Terrain = model;

            scenario.Chunks[srcChunk.PosX, srcChunk.PosZ] = chunk;
        }

        /*
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

        return scenario;

    }

    static Material[] GetTerrainMaterials(QadFile qad, Texture[] textures)
    {
        if (qad.UseMaterialTypeCT) { 
            return GetTerrainMaterialsC11(qad, textures);
        }
        else
        {
            return GetTerrainMaterialsWR2(qad, textures);
        }
    }

    static Material[] GetTerrainMaterialsC11(QadFile qad, Texture[] textures)
    {
        var materials = new Material[qad.MaterialsCT.Length];

        for (var i = 0; i < qad.MaterialsCT.Length; i++)
        {
            var matInfo = qad.MaterialsCT[i];
            var mat = new Material();

            mat.GameVersion = qad.GameVersion;
            mat.U16ShaderType0 = matInfo.L0Mode;
            mat.U16ShaderType1 = matInfo.L1Mode;

            if (matInfo.L0Tex0Id < textures.Length)
                mat.TexSlot0.Enable(textures[matInfo.L0Tex0Id]);

            if (matInfo.L0Tex1Id < textures.Length)
                mat.TexSlot1.Enable(textures[matInfo.L0Tex1Id]);

            if (matInfo.L0Tex2Id < textures.Length)
                mat.TexSlot2.Enable(textures[matInfo.L0Tex2Id]);

            if (matInfo.L1Tex0Id < textures.Length)
                mat.TexSlot3.Enable(textures[matInfo.L1Tex0Id]);

            if (matInfo.L1Tex1Id < textures.Length)
                mat.TexSlot4.Enable(textures[matInfo.L1Tex1Id]);

            if (matInfo.L1Tex2Id < textures.Length)
                mat.TexSlot5.Enable(textures[matInfo.L1Tex2Id]);

            materials[i] = mat;
        }

        return materials;
    }

        static Material[] GetTerrainMaterialsWR2(QadFile qad, Texture[] textures)
    {
        var materials = new Material[qad.MaterialsWR.Length];

        for (var i = 0; i < qad.MaterialsWR.Length; i++)
        {
            var matInfo = qad.MaterialsWR[i];
            var mat = new Material();

            mat.GameVersion = qad.GameVersion;
            mat.U16ShaderType0 = matInfo.Mode;

            if (matInfo.Tex0Id < textures.Length)
                mat.TexSlot0.Enable(textures[matInfo.Tex0Id]);

            if (matInfo.Tex1Id < textures.Length)
                mat.TexSlot1.Enable(textures[matInfo.Tex1Id]);

            if (matInfo.Tex2Id < textures.Length)
                mat.TexSlot2.Enable(textures[matInfo.Tex2Id]);

            materials[i] = mat;
        }
        return materials;
    }

    static void BuildTerrainChunks()
    {

    }
}
