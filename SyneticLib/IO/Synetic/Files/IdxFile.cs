using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGL.IO;

namespace SyneticLib.IO.Synetic.Files;
internal class IdxFile : SyneticBinaryFile, IIndexData
{
    public ushort[] Indices { get; set; }

    public Vector3Int[] Polygons { get; set; }

    public override void ReadFromView(BinaryViewReader br)
    {
        Indices = br.ReadArray<ushort>(LengthPrefix.Int32);

        Polygons = new Vector3Int[Indices.Length / 3];

        for (int i = 0; i < Polygons.Length; i++)
        {
            Polygons[i] = new Vector3Int(Indices[i * 3 + 0], Indices[i * 3 + 2], Indices[i * 3 + 1]);
        }
    }

    public override void WriteToView(BinaryViewWriter bw)
    {
        bw.WriteArray(Indices, LengthPrefix.Int32);
    }
}
