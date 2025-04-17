using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Grille.IO;
using SyneticLib.Files.Common;

using OpenTK.Mathematics;
using static SyneticLib.Files.MoxFile;

namespace SyneticLib.Files;
public class TrxFile : BinaryFile, IVertexData, IIndexData
{
    public MHead Head;
    public MPaintRegionInt32[] PaintRegions;

    public Vertex[] Vertecis { get; set; }
    public IdxTriangleInt32[] Triangles { get; set; }

    public MMaterial[] Materials;

    public TrxFile()
    {
        PaintRegions = Array.Empty<MPaintRegionInt32>();
        Vertecis = Array.Empty<Vertex>();
        Triangles = Array.Empty<IdxTriangleInt32>();
        Materials = Array.Empty<MMaterial>();
    }

    public static Vertex[] ReadVertecis(BinaryViewReader br, int count)
    {
        var vertecis = new Vertex[count];
        for (int i = 0; i < count; i++)
        {
            vertecis[i] = (Vertex)br.Read<MVertex>();
        }
        return vertecis;
    }

    public override void Deserialize(BinaryViewReader br)
    {
        Head = br.Read<MHead>();

        if (Head.Magic != "!XRT")
        {
            throw new InvalidDataException($"Invalid Head '{Head.Magic}'.");
        }

        br.Position += Head.LodCount * sizeof(int);
        Vertecis = ReadVertecis(br, Head.VtxCount);
        Triangles = ReadIndices(br, Head.PolyCount, 0);
        PaintRegions = br.ReadArray<MPaintRegionInt32>(Head.PaintRegionCount);
        br.Position += Head.LodCount * sizeof(ushort) * 4;

        Materials = br.ReadArray<MMaterial>(Head.MatCount);

        if (br.Position != br.Length)
        {
            throw new InvalidDataException();
        }
    }

    public override void Serialize(BinaryViewWriter bw)
    {
        bw.Write(Head);
        WriteVertecis(bw, Vertecis);
        bw.Position += 12;
        WriteIndices(bw, Triangles, 0);
        bw.WriteArray(PaintRegions, LengthPrefix.None);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MHead
    {
        public String4 Magic;
        public MVersion Version;
        public int VtxCount, PolyCount, PaintRegionCount, LodCount, U1, U2, U3, U4, MatCount, U5;
    }

    [StructLayout(LayoutKind.Sequential, Size = 40)]
    public struct MVertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 UV0;
        public Vector2 UV1;

        public static explicit operator Vertex(MVertex src) => new Vertex()
        {
            Position = src.Position,
            Normal = src.Normal,
            UV0 = src.UV0,
            //UV1 = src.UV1,
        };

        public static explicit operator MVertex(Vertex src) => new MVertex()
        {
            Position = src.Position,
            Normal = src.Normal,
            //UV0 = src.UV0,
            //UV1 = src.UV1,
        };
    }

    public struct MPaintRegionInt32
    {
        public int SidA;
        public int SidB;
        public int VtxOffset, VtxCount, IdxOffset, PolyCount;
    }
}
