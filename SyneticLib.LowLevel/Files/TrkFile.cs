using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.IO;

using OpenTK.Mathematics;

using SyneticLib.Files.Common;

namespace SyneticLib.Files;
public class TrkFile : BinaryFile
{
    public MHead Head;

    public MHead2 Head2;

    public MNode[] Nodes;

    public MTurn[] Turns;

    public MArrow[] Arrows;

    public TrkFile()
    {
        Nodes = Array.Empty<MNode>();
        Turns = Array.Empty<MTurn>();
        Arrows = Array.Empty<MArrow>();
    }


    public override void Deserialize(BinaryViewReader br)
    {
        Head = br.Read<MHead>();
        Nodes = br.ReadArray<MNode>(Head.Nodes);
        return;
        Head2 = br.Read<MHead2>();
        Turns = br.ReadArray<MTurn>(Head2.Turns);
        Arrows = br.ReadArray<MArrow>(Head2.Arrows);
    }

    public override void Serialize(BinaryViewWriter bw)
    {
        throw new NotImplementedException();
    }

    public struct MHead
    {
        public int Nodes;
        public int Loop;
        public ushort u1, u2, u3, u4;
    }

    public struct MNode
    {
        public Vector3 Position;
        public float Delta, CurveRad;
        public Matrix3 matrix;
        public float Ideal, Delta2;
        public ushort Margin1, Margin2, Tunnel, Column;
        public byte v1, v2, v3, v4;
    }

    public struct MHead2
    {
        public ushort a1, a2, Turns, a4, Arrows, a6, a7, a8;
    }

    public struct MTurn
    {
        public ushort Node1, Node2, Arrow1, ArrowCount, BitFlag, u1;
    };

    public struct MArrow
    {
        public Vector3 Pos;
        public Matrix3 mat;
        public float Delta;
        public int Flag;
    };
}
