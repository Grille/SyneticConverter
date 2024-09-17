using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.IO;
using Grille.IO.Interfaces;

using OpenTK.Mathematics;

using SyneticLib.Files.Common;
using System.Runtime.InteropServices;

namespace SyneticLib.Files;

public class CpoFile : BinaryFile
{
    public const int Magic = 0x43504F21;
    public MHead Head;
    MCollider[] Collider;

    public CpoFile()
    {
        Collider = Array.Empty<MCollider>();
    }

    public override void Deserialize(BinaryViewReader br)
    {
        int magic = br.ReadInt32();
        if (magic != Magic) {
            throw new InvalidDataException();
        }
        Head = br.Read<MHead>();

        Collider = new MCollider[Head.Count];

        for (int i = 0; i < Collider.Length; i++)
        {
            Collider[i] = br.ReadIView<MCollider>();
        }
    }

    public override void Serialize(BinaryViewWriter bw)
    {
        throw new NotImplementedException();
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MBox
    {
        Vector3 Scale;
        Vector3 Position;
        Matrix3 Matrix;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MShape
    {
        public int VerticeCount;
        public int PolyCount;
        public int IndiceSize;
        public int Clear1;
        public Vector3[] Vertices;
        public ushort[] Indices;
        public Vector3 Position;
        public Matrix3 Matrix;
    }

    public enum MFormat : int
    {
        None = 0,
        Box = 2,
        Shape = 3,
    }

    public unsafe class MCollider : IBinaryViewObject
    {
        public MFormat Format;
        public MBox Box;
        public MShape Shape;

        public void ReadFromView(BinaryViewReader br)
        {
            Format = br.Read<MFormat>();
            if (Format == MFormat.Box)
            {
                Box = br.Read<MBox>();
            }
            else if (Format == MFormat.Shape)
            {
                Shape.VerticeCount = br.ReadInt32();
                Shape.PolyCount = br.ReadInt32();
                Shape.IndiceSize = br.ReadInt32();
                Shape.Clear1 = br.ReadInt32();
                Shape.Vertices = br.ReadArray<Vector3>(Shape.VerticeCount);
                Shape.Indices = br.ReadArray<ushort>(Shape.IndiceSize*2);
                Shape.Position = br.Read<Vector3>();
                Shape.Matrix = br.Read<Matrix3>();
            }
            else
            {
                throw new InvalidDataException();
            }
        }

        public void WriteToView(BinaryViewWriter bw)
        {
            throw new NotImplementedException();
        }
    }

    public struct MHead
    {
        public int Count;
        public int U0;
        public int U1;
    }
}
