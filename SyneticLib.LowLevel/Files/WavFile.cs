using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.IO;

using SyneticLib.Files.Common;

namespace SyneticLib.Files;

public class WavFile : BinaryFile
{
    public byte[] Data { get; set; }

    public WavFile()
    {
        Data = Array.Empty<byte>();
    }

    public override void Deserialize(BinaryViewReader br)
    {
        Data = br.ReadRemainder();
    }

    public override void Serialize(BinaryViewWriter bw)
    {
        bw.WriteArray(Data, LengthPrefix.None);
    }
}
