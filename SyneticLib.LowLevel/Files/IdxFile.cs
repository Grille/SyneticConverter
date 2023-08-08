using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGL.IO;

namespace SyneticLib.LowLevel.Files;
public class IdxFile : BinaryFile, IIndexData
{
    public IndexTriangle[] Indices { get; set; }

    public override void ReadFromView(BinaryViewReader br)
    {
        var indices = br.ReadArray<ushort>(LengthPrefix.Int32);

        Indices = new IndexTriangle[indices.Length / 3];

        for (int i = 0; i < Indices.Length; i++)
        {
            Indices[i] = new IndexTriangle(indices[i * 3 + 0], indices[i * 3 + 2], indices[i * 3 + 1]);
        }
    }

    public override void WriteToView(BinaryViewWriter bw)
    {

        var indices = new ushort[Indices.Length * 3];

        for (int i = 0; i < Indices.Length; i++)
        {
            indices[i * 3 + 0] = (ushort)Indices[i].X;
            indices[i * 3 + 2] = (ushort)Indices[i].Y;
            indices[i * 3 + 1] = (ushort)Indices[i].Z;
        }

        bw.WriteArray(indices, LengthPrefix.Int32);
    }
}
