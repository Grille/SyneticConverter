using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using GGL.IO;
using System.Runtime.InteropServices;

namespace SyneticLib.IO.Synetic.Files;
public class QadFileWR1 : QadFile<QadFileWR1.MObjProp, QadFileWR1.MLight>
{
    public struct MObjProp
    {
        public ushort Mode;
    }

    public struct MLight
    {
        public Vector3 Position;
        public BgraColor Color;
    }
}

