using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticConverter;
public class MeshImporterMox : MeshImporter
{
    private GameVersion format;
    private MoxFile mox;

    public MeshImporterMox(Mesh target) : base(target)
    {
        this.target = target;
        mox = new();
    }

    public override void Load()
    {
        mox.Load(path);
    }
    public override void Assign()
    {
        target.Vertices = new Vertex[mox.Vertecis.Length];
        for (int i = 0; i < mox.Vertecis.Length; i++)
        {
            ref var srcvtx = ref mox.Vertecis[i];
            var vertex = target.Vertices[i] = new Vertex();

            vertex.Position = srcvtx.Position;
        }

        target.Poligons = new Vector3Int[mox.Indices.Length];
        for (int i = 0; i < mox.Indices.Length; i++)
        {
            target.Poligons[i].X = mox.Indices[i].X;
            target.Poligons[i].Y = mox.Indices[i].Y;
            target.Poligons[i].Z = mox.Indices[i].Z;
        }
    }

}
