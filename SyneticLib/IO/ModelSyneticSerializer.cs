using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;
using SyneticLib.IO.Generic;
using SyneticLib.Locations;

namespace SyneticLib.IO;
public class ModelSyneticSerializer : DirectoryFileSerializer<Model>
{
    public static Model Load(MoxFile mox, MtlFile mtl, TextureDirectory textures)
    {
        var materials = new List<Material>();

        foreach (var pair in mtl.Sections)
        {
            var srcMtl = pair.Value;
            var dstMat = new ModelMaterial();

            //dstMat.Diffuse = srcMtl.Diffuse[0];

            dstMat.TextureSlots[0].TryEnableByFile(textures, srcMtl.Tex1Name.Object);

            materials.Add(dstMat);
        }

        var mesh = new IndexedMesh(mox.Vertecis, mox.Triangles);

        var regions = new ModelMaterialRegion[mox.PaintRegions.Length];
        for (var i = 0; i < mox.PaintRegions.Length; i++)
        {
            ref var src = ref mox.PaintRegions[i];
            Material material;
            if (src.MatId >= materials.Count)
                material = materials[0];
            else
                material = materials[src.MatId];
            regions[i] = new ModelMaterialRegion(src.PolyOffset, src.PolyCount, material);
        }

        var submesh = new MeshSegment(mesh);

        return new Model(submesh, regions);
    }

    protected override Model OnLoad(string dirPath, string fileName)
    {
        var path = Path.Combine(dirPath, fileName);

        var moxPath = Path.ChangeExtension(path, "mox");
        var mtlPath = Path.ChangeExtension(path, "mtl");

        var mox = new MoxFile();
        var mtl = new MtlFile();

        mox.Load(moxPath);
        mtl.Load(mtlPath);

        var texPath = Path.Join(dirPath, "textures");
        if (!Directory.Exists(texPath))
        {
            texPath = Path.Join(dirPath, "textures_pc");
        }
        var textures = new TextureDirectory(texPath);

        return Load(mox, mtl, textures);
    }

    protected override void OnSave(string dirPath, string fileName, Model obj)
    {
        throw new NotImplementedException();
    }
}
