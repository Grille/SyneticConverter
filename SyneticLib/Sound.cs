using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib;

public class Sound : RessourceFile
{
    private byte[] buffer;

    protected override void OnLoad(string srcPath)
    {
        buffer = File.ReadAllBytes(srcPath);
    }

    protected override void OnSave(string dstPath)
    {
        File.WriteAllBytes(dstPath, buffer);
    }
}
