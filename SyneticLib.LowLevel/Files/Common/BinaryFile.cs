using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Grille.IO;
using SyneticLib.LowLevel.Compression;
using System.Diagnostics.CodeAnalysis;

namespace SyneticLib.Files.Common;
public abstract class BinaryFile : BaseFile
{
    public override sealed void Deserialize(Stream stream)
    {
        using var br = new BinaryViewReader(stream);
        Deserialize(br);
    }

    public override sealed void Serialize(Stream stream)
    {
        using var bw = new BinaryViewWriter(stream);
        Serialize(bw);
    }

    public abstract void Deserialize(BinaryViewReader br);

    public abstract void Serialize(BinaryViewWriter bw);
}
