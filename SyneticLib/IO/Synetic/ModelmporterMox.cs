using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using SyneticLib.IO.Synetic.Files;

namespace SyneticLib.IO.Synetic;
public class ModelmporterMox : MeshImporter
{
    protected new Model Target { get => (Model)base.Target; set => base.Target = value; }

    private MoxFile mox;
    private MtlFile mtl;

    public ModelmporterMox(Model target) : base(target)
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
     }
    protected override void OnAssign()
    {
        for (int i = 0; i< mox.Head.MatCount; i++)
        {
            var src = mtl.Sections[i];
            var dst = new ModelMaterial(Target);
            dst.Diffuse = BgraColor.FromInt(src.Diffuse[0]);


            var tex0 = Target.Textures.FindFileName(src.Tex1Name);
            dst.TexSlot0.Texture = tex0;
            dst.DataState = DataState.Loaded;
            Target.Materials.Add(dst);
        }


        Target.Vertices = mox.Vertecis;
        Target.Poligons = mox.Polygons;

        Target.MaterialRegion = new MaterialRegion[mox.Textures.Length];
        for (int i = 0; i < mox.Textures.Length; i++)
        {
            ref var src = ref mox.Textures[i];
            Target.MaterialRegion[i] = new MaterialRegion(src.PolyOffset, src.PolyCount, Target.Materials[src.MatId]);
        }
    }

}
