using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;
using SyneticLib.Files.Common;

namespace SyneticLib.IO;
public class MeshFileSerializer<TFile> : FileSerializer<TFile, IndexedMesh> where TFile : BaseFile, IVertexData, IIndexData, new()
{
    protected override IndexedMesh OnDeserialize(TFile cob)
    {
        return new IndexedMesh(cob.Vertecis, cob.Indices);
    }

    protected override void OnSerialize(TFile cob, IndexedMesh mesh)
    {
        cob.Vertecis = mesh.Vertices;
        cob.Indices = mesh.Triangles;
    }
}
