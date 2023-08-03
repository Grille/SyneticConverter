using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using SyneticLib.LowLevel.Files;
using SyneticLib.Locations;

namespace SyneticLib.IO;

public static partial class Imports
{
    public static Model LoadModelFromMox(string path)
    {
        var texpath = Path.Combine(path, "textures");
        var textures = new TextureDirectory(texpath);

        return LoadModelFromMox(path, textures);
    }

    public static Model LoadModelFromMox(string path, TextureDirectory textures)
    {
        var mox = new MoxFile();
        var mtl = new MtlFile();

        mox.Path = path;
        mtl.Path = Path.Join(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + ".mtl");

        mox.Load();
        mtl.Load();

        var materials = new List<Material>();

        for (var i = 0; i < mox.Head.MatCount; i++)
        {
            if (i >= mtl.Materials.Count)
            {
                //Target.Materials.Add(dstMat);
            }
            else
            {
                var srcMtl = mtl.Materials[i];
                var dstMat = new Material("");
                //dstMat.Diffuse = BgraColor.FromInt(srcMtl.Diffuse[0]);

                dstMat.TexSlot0.TryEnableByFile(textures, srcMtl.Tex1Name);
                //dstMat.TexSlot1.TryEnableByFile(Target.AssignedTextures, srcMtl.Tex2Name);
                //dstMat.TexSlot2.TryEnableByFile(Target.AssignedTextures, srcMtl.Tex3Name);

                materials.Add(dstMat);
            }
        }

        var mesh = new Mesh("", mox.Vertecis, mox.Indices);

        var regions = new ModelMaterialRegion[mox.Textures.Length];
        for (var i = 0; i < mox.Textures.Length; i++)
        {
            ref var src = ref mox.Textures[i];
            Material material;
            if (src.MatId >= materials.Count)
                material = materials[0];
            else
                material = materials[src.MatId];
            regions[i] = new ModelMaterialRegion(src.PolyOffset, src.PolyCount, material);
        }

        return new Model("", mesh, regions);
    }

}
