using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;
using SyneticLib.Files.Common;

namespace SyneticLib.IO;
public class MeshFileSerializer<TFile> : FileSerializer<TFile, Mesh> where TFile : BaseFile, IVertexData, IIndexData, new()
{
    protected override Mesh OnDeserialize(TFile cob)
    {
        return new Mesh(cob.Vertecis, cob.Indices);
    }

    protected override void OnSerialize(TFile cob, Mesh mesh)
    {
        cob.Vertecis = mesh.Vertices;
        cob.Indices = mesh.Indices;
    }
}
