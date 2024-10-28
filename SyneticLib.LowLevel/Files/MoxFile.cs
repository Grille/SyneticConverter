using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using Grille.IO;
using System.Runtime.InteropServices;
using SyneticLib.Files.Common;

namespace SyneticLib.Files;
public class MoxFile : BinaryFile, IVertexData, IIndexData
{
    public MHead Head;
    public MPaintRegionInt32[] PaintRegions;

    public Vertex[] Vertecis { get; set; }
    public IdxTriangleInt32[] Indices { get; set; }

    public MPart[]? Parts;
    public MLight[]? Lights;

    public MMaterial[] Materials;

    public MoxFile()
    {
        PaintRegions = Array.Empty<MPaintRegionInt32>();
        Vertecis = Array.Empty<Vertex>();
        Indices = Array.Empty<IdxTriangleInt32>();
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

    private static IdxTriangleInt32[] ReadU16Indices(BinaryViewReader br, int count)
    {
        var indicesU16 = br.ReadArray<IdxTriangleUInt16>(count);
        var indicesI32 = new IdxTriangleInt32[count];
        for (int i = 0; i < count; i++)
        {
            indicesI32[i] = indicesU16[i];
        }
        return indicesI32;
    }

    public static IdxTriangleInt32[] ReadIndices(BinaryViewReader br, int count, int version) => version switch
    {
        0 => ReadU16Indices(br, count),
        1 => br.ReadArray<IdxTriangleInt32>(count),
        _ => throw new InvalidDataException(),
    };

    private static MPaintRegionInt32[] ReadU16PaintRegions(BinaryViewReader br, int count)
    {
        var i32 = new MPaintRegionInt32[count];
        for (int i = 0; i < count; i++)
        {
            i32[i] = (MPaintRegionInt32)br.Read<MPaintRegionUInt16>();
        }
        return i32;
    }

    public static MPaintRegionInt32[] ReadPaintRegsions(BinaryViewReader br, int count, int version) => version switch
    {
        0 => ReadU16PaintRegions(br, count),
        2 => br.ReadArray<MPaintRegionInt32>(count),
        _ => throw new InvalidDataException(),
    };

    public override void Deserialize(BinaryViewReader br)
    {
        Head = br.Read<MHead>();

        if (Head.Magic != "!XOM")
        {
            throw new InvalidDataException($"Invalid Head '{Head.Magic}'.");
        }

        Vertecis = ReadVertecis(br, Head.VtxCount);
        Indices = ReadIndices(br, Head.PolyCount, Head.Version.V0IndexMode);
        PaintRegions = ReadPaintRegsions(br, Head.PaintRegionCount, Head.Version.V3ChunkMode);

        Materials = br.ReadArray<MMaterial>(Head.MatCount);
    }

    public static void WriteVertecis(BinaryViewWriter bw, Vertex[] vertices)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            bw.Write((MVertex)vertices[i]);
        }
    }

    public static void WriteIndices(BinaryViewWriter bw, IdxTriangleInt32[] indices, int version)
    {
        if (version == 0)
        {
            for (int i = 0; i < indices.Length; i++)
            {
                bw.Write((IdxTriangleUInt16)indices[i]);
            }
        }
        else if (version == 1)
        {
            bw.WriteArray(indices, LengthPrefix.None);
        }
        else
        {
            throw new ArgumentException(nameof(version));
        }
    }

    public static void WritePaintRegions(BinaryViewWriter bw, MPaintRegionInt32[] regions, int version)
    {
        if (version == 0)
        {
            for (int i = 0; i < regions.Length; i++)
            {
                bw.Write((MPaintRegionUInt16)regions[i]);
            }
        }
        else if (version == 1)
        {
            bw.WriteArray(regions, LengthPrefix.None);
        }
        else
        {
            throw new ArgumentException(nameof(version));
        }
    }

    public override void Serialize(BinaryViewWriter bw)
    {
        bw.Write(Head);

        WriteVertecis(bw, Vertecis);
        WriteIndices(bw, Indices, Head.Version.V0IndexMode);
        WritePaintRegions(bw, PaintRegions, Head.Version.V3ChunkMode);
        bw.WriteArray(Materials, LengthPrefix.None);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MHead
    {
        public String4 Magic;
        public MVersion Version;
        public int VtxCount, PolyCount, PaintRegionCount, MatCount, PartCount, LightCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MVersion
    {
        public byte V0IndexMode;
        public byte V1;
        public byte V2Extension;
        public byte V3ChunkMode;
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
            Position = src.Position,
            Normal = src.Normal,
            UV0 = src.UV,
            UV1 = Vector2.Zero,
        };

        public static explicit operator MVertex(Vertex src) => new MVertex()
        {
            Position = src.Position,
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
        public int Clear0;
        public int PolyOffset, PolyCount, VertBegin, VertEnd;
    }

    public struct MPaintRegionUInt16
    {
        public ushort MatId;
        public ushort Clear0;
        public ushort PolyOffset, PolyCount, VertBegin, VertEnd;

        public static explicit operator MPaintRegionUInt16(MPaintRegionInt32 a) => new MPaintRegionUInt16()
        {
            MatId = (ushort)a.MatId,
            Clear0 = (ushort)a.Clear0,
            PolyOffset = (ushort)a.PolyOffset,
            PolyCount = (ushort)a.PolyCount,
            VertBegin = (ushort)a.VertBegin,
            VertEnd = (ushort)a.VertEnd,
        };

        public static explicit operator MPaintRegionInt32(MPaintRegionUInt16 a) => new MPaintRegionInt32()
        {
            MatId = a.MatId,
            Clear0 = a.Clear0,
            PolyOffset = a.PolyOffset,
            PolyCount = a.PolyCount,
            VertBegin = a.VertBegin,
            VertEnd = a.VertEnd,
        };
    }

    public struct MPart
    {

    }

    public struct MLight
    {

    }

    [StructLayout(LayoutKind.Sequential, Size = 336)]
    public struct MMaterial
    {
        public int ID;
    }
}
