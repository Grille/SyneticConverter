using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using SyneticLib.Collections;
using SyneticLib.Files;
using SyneticLib.Files.Extra;
using SyneticLib.IO.Generic;
using SyneticLib.World;

using static SyneticLib.IO.Serializers;

namespace SyneticLib.IO.Extra;
public class ModelWavefrontSerializer : DirectoryFileSerializer<Model>
{
    protected override Model OnLoad(string dirPath, string fileName)
    {
        throw new NotImplementedException();
    }

    protected override void OnSave(string dirPath, string fileName, Model model)
    {
        var obj = new WavefrontObjFile();
        var mtl = new WavefrontMtlFile();

        var texturesdir = Path.Combine(dirPath, "Textures");
        var objpath = Path.Combine(dirPath, Path.ChangeExtension(fileName, ".obj"));
        var mtlpath = Path.Combine(dirPath, Path.ChangeExtension(fileName, ".mtl"));

        obj.Vertecis = model.MeshSection.Mesh.Vertices;
        obj.Triangles = model.MeshSection.Mesh.Triangles;

        var materials = model.GetMaterials().ToArray();
        var textures = model.GetTextures().ToArray();

        var materialMap = new Dictionary<Material, WavefrontMtlFile.MtlMaterial>();

        var mtlmaterials = new WavefrontMtlFile.MtlMaterial[materials.Length];
        for (int i = 0; i < mtlmaterials.Length; i++)
        {
            var src = materials[i];
            var dst = mtlmaterials[i] = new WavefrontMtlFile.MtlMaterial();

            var tex0 = src.TextureSlots[0].Texture;

            if (src is TerrainMaterial tsrc)
            {

            }
            else
            {

            }


            if (tex0 != null)
            {
                dst.Name = $"Mat_{i}_{tex0.Name}";
                dst.DiffuseMap = $"./textures/{tex0.Name}.dds";
            }
            else
            {
                dst.Name = $"Mat_{i}_null";
            }

            materialMap.Add(src, dst);
        }

        mtl.Materials = mtlmaterials;

        var matidxdict = new IndexDict<Material>();

        var objregions = new WavefrontObjFile.MtlRegion[model.MaterialRegions.Length];
        for (int i = 0; i < objregions.Length; i++)
        {
            var src = model.MaterialRegions[i];
            ref var dst = ref objregions[i];

            dst.Offset = src.ElementStart;
            dst.Length = src.ElementCount;
            dst.Material = materialMap[src.Material].Name;
        }

        var objmodel = new WavefrontObjFile.ObjModel("Terrain", objregions);
        obj.Models = new WavefrontObjFile.ObjModel[1] { objmodel };

        obj.Save(objpath);
        mtl.Save(mtlpath);

        Directory.CreateDirectory(texturesdir);
        foreach (var texture in textures)
        {
            var texFilePath = Path.Combine(texturesdir, $"{texture.Name}.dds");
            Serializers.Texture.Dds.Save(texFilePath, texture);
        }
    }
}
