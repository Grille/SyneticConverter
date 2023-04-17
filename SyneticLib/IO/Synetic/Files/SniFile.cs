using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using GGL.IO;
using System.Runtime.InteropServices;

namespace SyneticLib.IO.Synetic.Files;
public class SniFile : FileBinary
{
    public MHead Head;

    public MObj[] Objects;
    public MNode[] Nodes;

    public override void ReadFromView(BinaryViewReader br)
    {
        Head = br.Read<MHead>();

        Objects = br.ReadArray<MObj>(Head.ObjCount);
        Nodes = br.ReadArray<MNode>(Head.NodeCount);
    }

    public override void WriteToView(BinaryViewWriter bw)
    {
        bw.Write(Head);

        bw.WriteArray(Objects, LengthPrefix.None);
        bw.WriteArray(Nodes, LengthPrefix.None);
    }

    public struct MHead
    {
        public int ObjCount;
        public int NodeCount;
        public int X;
        public int Y;
    }

    public unsafe struct MObj
    {
        public ushort NodeCount;
        public ushort Id;
        public ushort NodeOffset;
        public ushort Mode;
        public String32 SoundFile;
        public ushort Volume;
        public ushort Tempo;
        public ushort Radius;
        public ushort X4;
    }

    public struct MNode
    {
        public Vector3 Position;
        public float Speed;
        public float Bezier;
    }
}


