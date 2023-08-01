using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using SyneticLib.LowLevel.Files;

namespace SyneticLib.IO;
public class ModelmporterMox
{
    protected new Model Target { get => (Model)base.Target; set => base.Target = value; }

    private MoxFile mox;
    private MtlFile mtl;

    public ModelmporterMox(Model target)
    {
        mox = new();
        mox.Path = target.SourcePath;

        mtl = new();
        mtl.Path = Path.Join(Path.GetDirectoryName(target.SourcePath), Path.GetFileNameWithoutExtension(target.SourcePath) + ".mtl");
    }

    protected override void OnLoad()
    {
        mox.Load();
        mtl.Load();

        for (var i = 0; i < mox.Head.MatCount; i++)
        {
            if (i >= mtl.Materials.Count)
            {
                //Target.Materials.Add(dstMat);
            }
            else
            {
                var srcMtl = mtl.Materials[i];
                var dstMat = new ModelMaterial(Target);
                //dstMat.Diffuse = BgraColor.FromInt(srcMtl.Diffuse[0]);

                dstMat.TexSlot0.TryEnableByFile(Target.AssignedTextures, srcMtl.Tex1Name);
                //dstMat.TexSlot1.TryEnableByFile(Target.AssignedTextures, srcMtl.Tex2Name);
                //dstMat.TexSlot2.TryEnableByFile(Target.AssignedTextures, srcMtl.Tex3Name);

                dstMat.DataState = DataState.Loaded;
                Target.Materials.Add(dstMat);
            }
        }


        Target.Mesh.Vertices = mox.Vertecis;
        Target.Mesh.Polygons = mox.Indices;

        Target.MaterialRegions = new ModelMaterialRegion[mox.Textures.Length];
        for (var i = 0; i < mox.Textures.Length; i++)
        {
            ref var src = ref mox.Textures[i];
            Material material;
            if (src.MatId >= Target.Materials.Count)
                material = Target.Materials[0];
            else
                material = Target.Materials[src.MatId];
            Target.MaterialRegions[i] = new ModelMaterialRegion(src.PolyOffset, src.PolyCount, material);
        }

        Target.DataState = DataState.Loaded;
    }

}
