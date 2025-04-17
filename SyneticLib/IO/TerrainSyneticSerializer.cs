using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;
using SyneticLib.IO.Generic;
using SyneticLib.Locations;
using SyneticLib.World;

namespace SyneticLib.IO;
public class TerrainSyneticSerializer : DirectoryFileSerializer<TerrainModel>
{
    public TerrainModel Load(ScenarioFiles files, TextureDirectory textures)
    {
        return BuildTerrain(files, textures);
    }

    protected override TerrainModel OnLoad(string dirPath, string fileName)
    {
        var files = new ScenarioFiles();
        files.Load(dirPath, fileName);

        var terrainTextures = new TextureDirectory(Path.Combine(dirPath, "textures"));
        return BuildTerrain(files, terrainTextures);
    }

    protected override void OnSave(string dirPath, string fileName, TerrainModel obj)
    {
        throw new NotImplementedException();
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

        var mesh = new IndexedMesh(vertecis, indices);


        var models = new Model[qad.Head.BlockCountX, qad.Head.BlockCountZ];

        for (int i = 0; i < qad.Chunks.Length; i++)
        {
            ref var srcChunk = ref qad.Chunks[i];

            int offset = offsets[srcChunk.MeshOffsetIndex];
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

}
