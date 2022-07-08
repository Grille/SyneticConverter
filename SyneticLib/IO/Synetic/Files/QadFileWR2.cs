using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using GGL.IO;
using System.Runtime.InteropServices;

namespace SyneticLib.IO.Synetic.Files;
public class QadFileWR2 : QadFile<QadFileWR2.MObjProp, QadFileWR2.MLight>
{
    public struct MObjProp
    {
        public ushort Mode, Shape, Weight, p4;
        public int x1, x2, x3;
        public String48 HitSound, FallSound;
    }

    public struct MLight
    {
        public int Mode;
        public float Size, Offset, Freq;
        public BgraColor Color;
        public byte b1, b2, b3, b4;
        public Matrix4x4 Matrix;
    }
}

