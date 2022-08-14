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

    public override void ReadFromView(BinaryViewReader br)
    {
        Indices = br.ReadArray<ushort>(LengthPrefix.Int32);
    }

    public override void WriteToView(BinaryViewWriter bw)
    {
        bw.WriteArray(Indices, LengthPrefix.Int32);
    }
}
