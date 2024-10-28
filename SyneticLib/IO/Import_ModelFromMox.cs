using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using SyneticLib;
using SyneticLib.Files;
using SyneticLib.Locations;

namespace SyneticLib.IO;

public static partial class Imports
{
    public static Model LoadModelFromMox(string filePath)
    {
        var dirPath = Path.GetDirectoryName(filePath);
        var texPath = Path.Join(dirPath, "textures");
        if (!Directory.Exists(texPath))
        {
            texPath = Path.Join(dirPath, "textures_pc");
        }
        var textures = new TextureDirectory(texPath);

        return LoadModelFromMox(filePath, textures);
    }

    public static Model LoadModelFromMox(string filePath, TextureDirectory textures)
    {
        var mox = new MoxFile();
        var mtl = new MtlFile();

        var moxPath = filePath;
        var mtlPath = Path.ChangeExtension(filePath, ".mtl");

        mox.Load(moxPath);
        mtl.Load(mtlPath);

        return LoadModelFromMox(mox, mtl, textures);
    }

    public static Model LoadModelFromMox(MoxFile mox, MtlFile mtl, TextureDirectory textures)
    {
        var materials = new List<Material>();

        for (var i = 0; i < mox.Head.MatCount; i++)
        {
            if (i >= mtl.Sections.Count)
            {
                //Target.Materials.Add(dstMat);
            }
            else
            {
                //var srcMtl = mtl.Sections[i];
                var dstMat = new ModelMaterial();

                //dstMat.Diffuse = BgraColor.FromInt(srcMtl.Diffuse[0]).ToNormalizedVector3();

                //dstMat.TextureSlots[0].TryEnableByFile(textures, srcMtl.Tex1Name);
                //dstMat.TextureSlots[1].TryEnableByFile(textures, srcMtl.Tex2Name);
                //dstMat.TextureSlots[2].TryEnableByFile(textures, srcMtl.Tex3Name);

                materials.Add(dstMat);
            }
        }

        var mesh = new Mesh(mox.Vertecis, mox.Indices);

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

}
