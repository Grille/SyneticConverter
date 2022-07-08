using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using GGL.IO;

namespace SyneticLib.IO.Synetic.Files;
public abstract class SyneticBinFile
{
    public abstract void Read(BinaryViewReader br);

    public abstract void Write(BinaryViewWriter bw);

    public void Load(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"file '{path}' not found", path);

        using var br = new BinaryViewReader(path);
        Read(br);
    }

    public void Save(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"file '{path}' not found", path);

        using var bw = new BinaryViewWriter(path);
        Write(bw);
    }
}
