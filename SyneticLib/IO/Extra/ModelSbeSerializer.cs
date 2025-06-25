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


namespace SyneticLib.IO.Extra;
public class ModelSbeSerializer : DirectoryFileSerializer<Model>
{
    protected override Model OnLoad(string dirPath, string fileName)
    {
        throw new NotImplementedException();
    }

    protected override void OnSave(string dirPath, string fileName, Model model)
    {
        var sbe = new SbeFile();

        var texturesdir = Path.Combine(dirPath, "Textures");
        var sbepath = Path.Combine(dirPath, Path.ChangeExtension(fileName, ".sbe"));

        sbe.Vertecis = model.MeshSection.Mesh.Vertices;
        sbe.Triangles = model.MeshSection.Mesh.Triangles;

        var materials = model.GetMaterials().ToArray();
        var textures = model.GetTextures().ToArray();

        var materialMap = new Dictionary<Material, SbeFile.SbeMaterial>();

        var sbematerials = new SbeFile.SbeMaterial[materials.Length];
        for (int i = 0; i < sbematerials.Length; i++)
        {
            var src = materials[i];
            var dst = sbematerials[i] = new SbeFile.SbeMaterial();

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
                dst.Textures[0] = $"./textures/{tex0.Name}.dds";
            }
            else
            {
                dst.Name = $"Mat_{i}_null";
            }

            materialMap.Add(src, dst);
        }

        sbe.Materials = sbematerials;

        var matidxdict = new IndexDict<Material>();

        var objregions = new SbeFile.SbeRegion[model.MaterialRegions.Length];
        for (int i = 0; i < objregions.Length; i++)
        {
            var src = model.MaterialRegions[i];
            ref var dst = ref objregions[i];

            dst.Offset = src.ElementStart;
            dst.Length = src.ElementCount;
            dst.Material = materialMap[src.Material].Name;
        }

        var objmodel = new SbeFile.SbeModel("Terrain", objregions);
        sbe.Models = new SbeFile.SbeModel[1] { objmodel };

        sbe.Save(sbepath);

        Directory.CreateDirectory(texturesdir);
        foreach (var texture in textures)
        {
            var texFilePath = Path.Combine(texturesdir, $"{texture.Name}.dds");
            Serializers.Texture.Dds.Save(texFilePath, texture);
        }
    }
}
