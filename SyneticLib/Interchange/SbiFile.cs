using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.IO;

using SyneticLib.Files.Common;

namespace SyneticLib.Interchange;

public class SbiFile : BinaryFile
{
    const int Magic = 0x53594e42;

    public override void Deserialize(BinaryViewReader br)
    {
        throw new NotImplementedException();
    }

    public override void Serialize(BinaryViewWriter bw)
    {
        throw new NotImplementedException();
    }

    public struct MHead
    {
        public int Magic;
        public int Version;
    }
}
