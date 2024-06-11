using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using Grille.IO;
using SyneticLib.Files.Common;

namespace SyneticLib.Files;

public class LvlFile : BinaryFile
{
    public MData Data;

    public override void Deserialize(BinaryViewReader br)
    {
        Data = br.Read<MData>();
    }

    public override void Serialize(BinaryViewWriter bw)
    {
        bw.Write(Data);
    }

    public struct MData
    {
        public float A, B, C;
        public Matrix3 Matrix;
        public Vector4 Sun;
    }
}


