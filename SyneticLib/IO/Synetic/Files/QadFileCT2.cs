using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using GGL.IO;
using System.Runtime.InteropServices;

namespace SyneticLib.IO.Synetic.Files;
public class QadFileCT2 : QadFileWR2
{
    public QadFileCT2()
    {
        Has8ByteMagic = true;
        Has56ByteBlock = true;
    }
}

