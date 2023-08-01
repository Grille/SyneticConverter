using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib.IO;
public static class SoundExport
{
    public static void Save(this Sound sound, string path)
    {
        File.WriteAllBytes(path, sound.Buffer);
    }
}
