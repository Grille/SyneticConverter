using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib;

public class Sound : Ressource
{
    public byte[] Buffer;

    public Sound(GameFolder parent, string path): base(parent, path, PointerType.File)
    {

    }

    protected override void OnLoad()
    {
        Buffer = File.ReadAllBytes(SourcePath);
    }

    protected override void OnSave()
    {
        File.WriteAllBytes(SourcePath, Buffer);
    }

    public void Play()
    {

    }

    protected override void OnSeek()
    {

    }
}
