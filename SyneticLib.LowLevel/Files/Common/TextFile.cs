using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.IO;

namespace SyneticLib.Files.Common;

public abstract class TextFile : BaseFile
{
    public override sealed void Deserialize(Stream stream)
    {
        using var br = new StreamReader(stream, leaveOpen: true);
        Deserialize(br);
    }

    public override sealed void Serialize(Stream stream)
    {
        using var bw = new StreamWriter(stream, leaveOpen: true);
        Serialize(bw);
    }

    public abstract void Deserialize(StreamReader reader);

    public abstract void Serialize(StreamWriter writer);
}
