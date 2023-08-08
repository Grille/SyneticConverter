using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using GGL.IO;
using System.Runtime.InteropServices;

namespace SyneticLib.LowLevel.Files;
public class MoxFile : BinaryFile, IVertexData, IIndexData
{
    public const int MBWR = 65536;
    public const int SimpleWR2 = 33554432;
    public const int ComplexWR2 = 33685504;

    public MHead Head;
    public MPaintRegionInt32[] Textures;

    public int[] IndicesOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Vertex[] Vertecis { get; set; }
    public IndexTriangle[] Indices { get; set; }

    public byte[] Rest;

    public unsafe override void ReadFromView(BinaryViewReader br)
    {
        Head = br.Read<MHead>();

        if (Head.Magic != "!XOM")
            throw new InvalidOperationException($"Invalid Head '{Head.Magic}'.");

        Vertecis = new Vertex[Head.VertCount];
        for (int i = 0; i < Vertecis.Length; i++)
        {
            Vertecis[i] = (Vertex)br.Read<MVertex>();
        }

        var indices = br.ReadArray<ushort>(Head.PolyCount * 3);
        Indices = new IndexTriangle[indices.Length / 3];
        for (int i = 0; i < Indices.Length; i++)
        {
            Indices[i] = new IndexTriangle(indices[i * 3 + 0], indices[i * 3 + 2], indices[i * 3 + 1]);
        }

        if (Head.Version == MBWR)
        {
            Textures = new MPaintRegionInt32[Head.TextureCount];
            for (int i = 0; i < Head.TextureCount; i++)
            {
                Textures[i] = (MPaintRegionInt32)br.Read<MPaintRegionUInt16>();
            }
        }
        else
        {
            Textures = br.ReadArray<MPaintRegionInt32>(Head.TextureCount);
        }

        var list = new List<byte>();
        br.ReadRemainderToIList(list, 0);
        Rest = list.ToArray();

        /*
        br.Seek(0x150 * Head.MatCount);
        br.Seek(0x0C4 * Head.PartCount);
        br.Seek(0x058 * Head.LightCount);
        */
    }

    public override void WriteToView(BinaryViewWriter bw)
    {
        bw.Write(Head);

        for (int i = 0; i < Vertecis.Length; i++)
        {
            bw.Write((MVertex)Vertecis[i]);
        }

        for (int i = 0; i < Head.PolyCount; i++)
        {
            bw.Write((ushort)Indices[i].X);
            bw.Write((ushort)Indices[i].Z);
            bw.Write((ushort)Indices[i].Y);
        }

        if (Head.Version == MBWR)
        {
            for (int i = 0; i < Head.TextureCount; i++)
            {
                bw.Write((MPaintRegionUInt16)Textures[i]);
            }
        }
        else
        {
            bw.WriteArray(Textures, LengthPrefix.None);
        }

        bw.WriteArray(Rest, LengthPrefix.None);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MHead
    {
        public String4 Magic;
        public int Version;
        public int VertCount, PolyCount, TextureCount, MatCount, PartCount, LightCount;
    }

    [StructLayout(LayoutKind.Sequential, Size = 40)]
    public struct MVertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 UV;
        public float c, d;

        public static explicit operator Vertex(MVertex src) => new Vertex()
        {
            InvPosition = src.Position,
            Normal = src.Normal,
            UV0 = src.UV,
            UV1 = Vector2.Zero,
        };

        public static explicit operator MVertex(Vertex src) => new MVertex()
        {
            Position = src.InvPosition,
            Normal = src.Normal,
            UV = src.UV0,
        };
    }

    public struct MPolygonUInt16
    {
        public ushort X, Y, Z;
    }

    public struct MPaintRegionInt32
    {
        public int MatId;
        public byte Flag0, Flag1;
        public ushort Clear0;
        public int PolyOffset, PolyCount, VertBegin, VertEnd;
    }

    public struct MPaintRegionUInt16
    {
        public ushort MatId;
        public byte Flag0, Flag1;
        public ushort PolyOffset, PolyCount, VertBegin, VertEnd;

        public static explicit operator MPaintRegionUInt16(MPaintRegionInt32 a) => new MPaintRegionUInt16()
        {
            MatId = (ushort)a.MatId,
            Flag0 = a.Flag0,
            Flag1 = a.Flag1,
            PolyOffset = (ushort)a.PolyOffset,
            PolyCount = (ushort)a.PolyCount,
            VertBegin = (ushort)a.VertBegin,
            VertEnd = (ushort)a.VertEnd,
        };

        public static explicit operator MPaintRegionInt32(MPaintRegionUInt16 a) => new MPaintRegionInt32()
        {
            MatId = a.MatId,
            Flag0 = a.Flag0,
            Flag1 = a.Flag1,
            PolyOffset = a.PolyOffset,
            PolyCount = a.PolyCount,
            VertBegin = a.VertBegin,
            VertEnd = a.VertEnd,
        };
    }
}
