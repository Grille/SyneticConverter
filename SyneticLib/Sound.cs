using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib;

public class Sound : SyneticObject
{
    public SoundFormat Type { get; }

    public byte[] Buffer { get; }

    public Sound(string name, SoundFormat type, byte[] buffer): base(name)
    {
        Type = type;
        Buffer = buffer;
    }
}
