using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;

namespace SyneticLib.IO;
public class MeshCobSerializer : FileSerializer<CobFile, Mesh>
{
    public override Mesh OnDeserialize(CobFile cob)
    {
        return new Mesh(cob.Vertecis, cob.Indices);
    }

    protected override void OnSerialize(CobFile cob, Mesh mesh)
    {
        cob.Vertecis = mesh.Vertices;
        cob.Indices = mesh.Indices;
    }
}
