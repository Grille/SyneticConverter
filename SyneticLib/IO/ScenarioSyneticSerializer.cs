using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;
using SyneticLib.Locations;
using SyneticLib.World;

namespace SyneticLib.IO;
public class ScenarioSyneticSerializer : DirectorySerializer<Scenario>
{
    protected override void OnSave(string dirPath, Scenario obj)
    {
        throw new NotImplementedException();
    }

    protected override Scenario OnLoad(string filePath)
    {
        var dirPath = Path.GetDirectoryName(filePath);
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        return Load(dirPath!, fileName);
    }

    public Scenario Load(string dirPath, string fileName)
    {
        var files = new ScenarioFiles();

        files.Load(dirPath, fileName);

        var syn = files.Syn;
        var lvl = files.Lvl;
        var sni = files.Sni;
        var qad = files.Qad;
        var sky = files.Sky;

        var terrainTextures = new TextureDirectory(Path.Combine(dirPath, "textures"));
        var modelTextures = new TextureDirectory(Path.Combine(dirPath, "objects/textures"));

        var scenario = new Scenario(0);

        scenario.Width = qad.Head.BlockCountX;
        scenario.Height = qad.Head.BlockCountZ;

        scenario.Chunks = new ScenarioChunk[qad.Head.BlockCountX, qad.Head.BlockCountZ];

        scenario.Terrain = BuildTerrain(files, terrainTextures);

        for (int i = 0; i < qad.Chunks.Length; i++)
        {
            var cinfo = new ScenarioChunkCreateInfo(qad.Chunks[i])
            {
                Terrain = scenario.Terrain,
            };
            scenario.Chunks[cinfo.Position.X, cinfo.Position.Z] = new ScenarioChunk(cinfo);
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

    static TerrainModel BuildTerrain(ScenarioFiles files, TextureDirectory terrainTextures)
    {
        var syn = files.Syn;
        var lvl = files.Lvl;
        var sni = files.Sni;
        var qad = files.Qad;
        var sky = files.Sky;

        var vertecis = files.TerrainMesh.Vertices;
        var indices = files.TerrainMesh.Indices;
        var iOffsets = files.TerrainMesh.Offsets;

        var indexedTextures = terrainTextures.CreateIndexedArray(qad.TextureNames);
        var terrainMaterials = GetTerrainMaterials(qad, indexedTextures);

        var offsets = new int[iOffsets.Length + 1];
        for (var i = 0; i < offsets.Length - 1; i++)
        {
            offsets[i + 1] = offsets[i] + iOffsets[i];
        }

        var mesh = new Mesh(vertecis, indices);


        var models = new Model[qad.Head.BlockCountX, qad.Head.BlockCountZ];

        for (int i = 0; i < qad.Chunks.Length; i++)
        {
            ref var srcChunk = ref qad.Chunks[i];

            int offset = offsets[srcChunk.Chunk65k];
            var submesh = new MeshSegment(mesh, srcChunk.Poly.Start, srcChunk.Poly.Length, offset);

            var regions = new ModelMaterialRegion[srcChunk.PolyRegion.Length];
            for (int iy = 0; iy < regions.Length; iy++)
            {
                var srcRegion = qad.PolyRegions[iy + srcChunk.PolyRegion.Start];
                var region = new ModelMaterialRegion(srcRegion.PolyOffset, srcRegion.PolyCount, terrainMaterials[srcRegion.SurfaceId1]);
                regions[iy] = region;
            }

            var model = new Model(submesh, regions);
            models[srcChunk.Position.X, srcChunk.Position.Z] = model;
        }

        return new TerrainModel(mesh, models);
    }

    static Material[] GetTerrainMaterials(QadFile qad, Texture[] textures)
    {
        var materials = new Material[qad.Materials.Length];

        for (var i = 0; i < qad.Materials.Length; i++)
        {
            var matInfo = qad.Materials[i];
            var mat = new TerrainMaterial();

            mat.GameVersion = qad.GameVersion;
            mat.Layer0.Mode = matInfo.Layer0.Mode;
            mat.Layer1.Mode = matInfo.Layer1.Mode;
            mat.Matrix0 = matInfo.Matrix0;
            mat.Matrix1 = matInfo.Matrix1;
            mat.Matrix2 = matInfo.Matrix2;

            EnableLayer(mat, 0, ref matInfo.Layer0, textures);
            EnableLayer(mat, 3, ref matInfo.Layer1, textures);

            materials[i] = mat;
        }

        return materials;
    }

    static void EnableLayer(Material material, int slot, ref QadFile.MMaterialLayer layer, Texture[] textures)
    {
        void EnableSlot(int slot, int textureId)
        {
            if (textureId < textures.Length)
            {
                material.TextureSlots[slot].Enable(textures[textureId]);
            }
        }

        EnableSlot(slot + 0, layer.Tex0Id);
        EnableSlot(slot + 1, layer.Tex1Id);
        EnableSlot(slot + 2, layer.Tex2Id);
    }

    static void BuildTerrainChunks()
    {

    }
}
