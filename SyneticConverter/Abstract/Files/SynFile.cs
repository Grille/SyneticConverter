using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGL.IO;

namespace SyneticConverter.Abstract.Files;
public class SynFile : SyneticBinFile
{
    MHead Head;
    public override void Read(BinaryViewReader br)
    {
        throw new NotImplementedException();
    }

    public override void Write(BinaryViewWriter bw)
    {
        throw new NotImplementedException();
    }

    public struct MHead
    {
        String8 Magic;
        uint FileCount;
        uint Empty;
    }

    public struct MFileEntry
    {
        public String48 FileName;
        public uint Position, SynSize, FileSize, Empty;
    }
}
