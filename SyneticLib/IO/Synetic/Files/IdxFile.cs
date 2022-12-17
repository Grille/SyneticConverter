using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGL.IO;

namespace SyneticLib.IO.Synetic.Files;
public class IdxFile : FileBinary, IIndexData
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

        Indices = new ushort[Polygons.Length * 3];

        for (int i = 0; i < Polygons.Length; i++)
        {
            Indices[i * 3 + 0] = (ushort)Polygons[i].X;
            Indices[i * 3 + 2] = (ushort)Polygons[i].Y;
            Indices[i * 3 + 1] = (ushort)Polygons[i].Z;
        }

        bw.WriteArray(Indices, LengthPrefix.Int32);
    }
}
