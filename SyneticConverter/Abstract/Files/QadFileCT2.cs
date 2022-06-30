using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using GGL.IO;
using System.Runtime.InteropServices;

namespace SyneticConverter;
public class QadFileCT2 : QadFileWR2
{
    public QadFileCT2()
    {
        HasMagic = true;
        Has56DataBlock = true;
    }
}

