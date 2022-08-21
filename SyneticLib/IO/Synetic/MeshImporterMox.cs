using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.IO.Synetic.Files;

namespace SyneticLib.IO.Synetic;
public class MeshImporterMox : MeshImporter
{
    private MoxFile mox;

    public MeshImporterMox(Mesh target) : base(target)
    {
        mox = new();
        mox.Path = target.SourcePath;
    }

    protected override void OnLoad()
    {
        mox.Load();
    }
    protected override void OnAssign()
    {
        target.Vertices = new MeshVertex[mox.Vertecis.Length];
        for (var i = 0; i < mox.Vertecis.Length; i++)
        {
            ref var srcvtx = ref mox.Vertecis[i];
            var vertex = target.Vertices[i] = new MeshVertex();

            vertex.Position = srcvtx.Position;
        }

        target.Poligons = new Vector3Int[mox.Indices.Length];
        for (var i = 0; i < mox.Indices.Length; i++)
        {
            target.Poligons[i].X = mox.Indices[i].X;
            target.Poligons[i].Y = mox.Indices[i].Y;
            target.Poligons[i].Z = mox.Indices[i].Z;
        }

        target.MaterialRegion = new MaterialRegion[] { new(0, target.Poligons.Length) { } };
    }

}
