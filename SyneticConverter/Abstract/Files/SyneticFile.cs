using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using GGL.IO;

namespace SyneticConverter;
public abstract class SyneticFile
{
    public abstract void Read(BinaryViewReader br);

    public abstract void Write(BinaryViewWriter bw);

    public void Load(string path)
    {
        using (var br = new BinaryViewReader(path))
        {
            Read(br);
        }
    }

    public void Save(string path)
    {
        using (var bw = new BinaryViewWriter(path))
        {
            Write(bw);
        }
    }
}
