using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib;

public class Sound : Ressource
{
    private byte[] buffer;

    public Sound(GameFolder parent, string name): base(parent, PointerType.File)
    {

    }

    protected override void OnLoad()
    {
        buffer = File.ReadAllBytes(SourcePath);
    }

    protected override void OnSave()
    {
        File.WriteAllBytes(SourcePath, buffer);
    }

    protected override void OnSeek()
    {
        throw new NotImplementedException();
    }
}
