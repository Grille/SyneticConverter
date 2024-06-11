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
    public static Model LoadModelFromMox(string path)
    {
        var dirpath = Path.GetDirectoryName(path);
        var texpath = Path.Combine(dirpath, "textures");
        if (!Directory.Exists(texpath))
        {
            texpath = Path.Combine(dirpath, "textures_pc");
        }
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
                var dstMat = new Material();

                //dstMat.Diffuse = BgraColor.FromInt(srcMtl.Diffuse[0]).ToNormalizedVector3();

                dstMat.TexSlot0.TryEnableByFile(textures, srcMtl.Tex1Name);
                dstMat.TexSlot1.TryEnableByFile(textures, srcMtl.Tex2Name);
                dstMat.TexSlot2.TryEnableByFile(textures, srcMtl.Tex3Name);

                materials.Add(dstMat);
            }
        }

        var mesh = new Mesh(mox.Vertecis, mox.Indices);

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

        var submesh = new MeshSegment(mesh);

        return new Model(submesh, regions);
    }

}
