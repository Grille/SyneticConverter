using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using GGL.IO;
using System.Runtime.InteropServices;
using static SyneticLib.IO.Synetic.Files.MoxFile;

namespace SyneticLib.IO.Synetic.Files;
public class MoxFile : SyneticBinaryFile, IVertexData, IIndexData
{
    public const int MBWR = 65536;
    public const int SimpleWR2 = 33554432;
    public const int ComplexWR2 = 33685504;

    public MHead Head;
    public MTex32[] Textures;

    public int[] VtxQty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Vertex[] Vertecis { get; set; }
    public Vector3Int[] Polygons { get; set; }

    public unsafe override void ReadFromView(BinaryViewReader br)
    {
        Head = br.Read<MHead>();

        if (Head.Magic != "!XOM")
            throw new InvalidOperationException($"Invalid Head '{Head.Magic}'.");

        Vertecis = new Vertex[Head.VertCount];
        for (int i = 0; i < Vertecis.Length; i++)
        {
            var src = br.Read<MVertex>();
            Vertecis[i] = new Vertex()
            {
                Position = new Vector3(src.Position.X, src.Position.Z, src.Position.Y),
                Normal = new Vector3(src.Normal.X, src.Normal.Y, src.Normal.Z/*, src.Normal.A / 255f*/),
                UV0 = src.UV,
                UV1 = Vector2.Zero,
            };
        }

        var indices = br.ReadArray<ushort>(Head.PolyCount * 3);
        Polygons = new Vector3Int[indices.Length / 3];
        for (int i = 0; i < Polygons.Length; i++)
        {
            Polygons[i] = new Vector3Int(indices[i * 3 + 0], indices[i * 3 + 2], indices[i * 3 + 1]);
        }

        if (Head.Version == MBWR)
        {
            Textures = new MTex32[Head.TextureCount];
            for (int i = 0; i < Head.TextureCount; i++)
            {
                Textures[i] = (MTex32)br.Read<MTex16>();
            }
        }
        else
        {
            Textures = br.ReadArray<MTex32>(Head.TextureCount);
        }

        /*
        br.Seek(0x150 * Head.MatCount);
        br.Seek(0x0C4 * Head.PartCount);
        br.Seek(0x058 * Head.LightCount);
        */
    }

    public override void WriteToView(BinaryViewWriter bw)
    {
        throw new NotImplementedException();
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
    }

    public struct MPoly
    {
        public ushort X, Y, Z;
    }

    public struct MTex32
    {
        public int MatId;
        public byte Flag0, Flag1;
        public ushort Clear0;
        public int PolyOffset, PolyCount, VertBegin, VertEnd;

        public static explicit operator MTex32(MTex16 a)
        {
            var b = new MTex32();
            b.MatId = a.MatId;
            b.Flag0 = a.Flag0;
            b.Flag1 = a.Flag1;
            b.PolyOffset = a.PolyOffset;
            b.PolyCount = a.PolyCount;
            b.VertBegin = a.VertBegin;
            b.VertEnd = a.VertEnd;
            return b;
        }
    }

    public struct MTex16
    {
        public ushort MatId;
        public byte Flag0, Flag1;
        public ushort PolyOffset, PolyCount, VertBegin, VertEnd;
    }
}
