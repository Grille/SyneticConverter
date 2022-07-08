using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using GGL.IO;

namespace SyneticLib.IO.Synetic.Files;

public class LvlFile : SyneticBinFile
{
    public MData Data;

    public override void Read(BinaryViewReader br)
    {
        Data = br.Read<MData>();
    }

    public override void Write(BinaryViewWriter bw)
    {
        bw.Write(Data);
    }

    public struct MData
    {
        public float A, B, C;
        public Matrix3x3 Matrix;
        public Vector4 Sun;
    }
}


