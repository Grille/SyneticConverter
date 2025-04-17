using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grille.IO;
using SyneticLib.Files.Common;

namespace SyneticLib.Files;
public class IdxFile : BinaryFile, IIndexData
{
    public IdxTriangleInt32[] Triangles { get; set; }

    public IdxFile()
    {
        Triangles = Array.Empty<IdxTriangleInt32>();
    }

    public override void Deserialize(BinaryViewReader br)
    {
        var indices = br.ReadArray<ushort>(LengthPrefix.Int32);

        Triangles = new IdxTriangleInt32[indices.Length / 3];

        for (int i = 0; i < Triangles.Length; i++)
        {
            Triangles[i] = new IdxTriangleInt32(indices[i * 3 + 0], indices[i * 3 + 1], indices[i * 3 + 2]);
        }
    }

    public override void Serialize(BinaryViewWriter bw)
    {

        var indices = new ushort[Triangles.Length * 3];

        for (int i = 0; i < Triangles.Length; i++)
        {
            indices[i * 3 + 0] = (ushort)Triangles[i].X;
            indices[i * 3 + 1] = (ushort)Triangles[i].Y;
            indices[i * 3 + 2] = (ushort)Triangles[i].Z;
        }

        bw.WriteArray(indices, LengthPrefix.Int32);
    }
}
