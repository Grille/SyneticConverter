using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using GGL.IO;
using System.Runtime.InteropServices;

namespace SyneticLib.IO.Synetic.Files;
public class GeoFile : SyneticBinaryFile, IIndexData, IVertexData
{
    /// <summary>Used by CT5</summary>
    public bool HasX16VertexBlock = false;

    public MHead Head;

    public int[] IndicesOffset { get; set; }
    public Vertex[] Vertecis { get; set; }
    private ushort[] Indices { get; set; }

    public Vector3Int[] Polygons { get; set; }

    public unsafe override void ReadFromView(BinaryViewReader br)
    {
        Head = br.Read<MHead>();

        IndicesOffset = br.ReadArray<int>(Head.IndicesOffsetCount);

        int vertexCount = getVertCount();
        assertFileSize(vertexCount, Head.IndicesCount, (int)br.Length);

        Vertecis = new Vertex[vertexCount];
        for (var i = 0; i < vertexCount; i++)
            Vertecis[i] = br.Read<MVertex>();

        if (HasX16VertexBlock)
            br.ReadArray<byte>(vertexCount * 16);

        Indices = br.ReadArray<ushort>(Head.IndicesCount);

        Polygons = new Vector3Int[Indices.Length / 3];

        for (int i = 0; i < Polygons.Length; i++)
            Polygons[i] = new Vector3Int(Indices[i * 3 + 0], Indices[i * 3 + 2], Indices[i * 3 + 1]);
    }

    public override void WriteToView(BinaryViewWriter bw)
    {
        bw.Write(Head);

        bw.WriteArray(IndicesOffset, LengthPrefix.None);

        var vertexCount = getVertCount();

        for (int i = 0; i < vertexCount; i++)
            bw.Write<MVertex>(Vertecis[i]);

        if (HasX16VertexBlock)
            bw.Seek(vertexCount * 16, SeekOrigin.Current);

        Indices = new ushort[Polygons.Length * 3];

        for (int i = 0; i < Polygons.Length; i++)
        {
            Indices[i * 3 + 0] = (ushort)Polygons[i].X;
            Indices[i * 3 + 2] = (ushort)Polygons[i].Y;
            Indices[i * 3 + 1] = (ushort)Polygons[i].Z;
        }

        bw.WriteArray(Indices, LengthPrefix.None);
    }

    private int getVertCount()
    {
        int vertexCount = 0;
        for (int i = 0; i < IndicesOffset.Length; i++)
            vertexCount += IndicesOffset[i];
        return vertexCount;
    }
    private unsafe int getEndPos(int vtxCount, int idxCount)
    {
        int calculatedEndPos = sizeof(MHead) + sizeof(int) * 64 + vtxCount * sizeof(MVertex) + idxCount * sizeof(ushort);
        if (HasX16VertexBlock)
            calculatedEndPos += vtxCount * 16;

        return calculatedEndPos;
    }

    private void assertFileSize(int vertexCount, int IndicesCount, int length)
    {
        int calculatedEndPos = getEndPos(vertexCount, IndicesCount);

        if (calculatedEndPos != length)
            throw new Exception($"Invalid File Size: ({calculatedEndPos} != {length}) Diff {calculatedEndPos - length}");

    }


    public void SetFlagsAccordingToVersion(GameVersion version)
    {
        HasX16VertexBlock = version >= GameVersion.CT5;
    }


    public void FillHead()
    {
        Head.Magic = (String4)"MOEG";
        Head.X0 = 1;
        Head.X1 = 1;
        Head.SectionCount = 2;
        Head.IndicesOffsetCount = IndicesOffset.Length;
        Head.IndicesCount = Polygons.Length * 3;
        Head.Clear0 = 0;
        Head.Clear1 = 0;
        Head.Clear2 = 0;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MHead
    {
        public String4 Magic;
        public ushort X0, X1;
        public int SectionCount, IndicesOffsetCount, IndicesCount;
        public int Clear0, Clear1, Clear2;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MVertex
    {
        public Vector3 Position;
        public BgraColor Normal;
        public Vector2 UV0;
        public Vector2 UV1;
        public BlendColor Blend;
        public BgraColor Color;

        public static implicit operator Vertex(MVertex a) => new Vertex()
        {
            InvPosition = a.Position,
            RGBAInvNormal = a.Normal,
            UV0 = a.UV0,
            UV1 = a.UV1,
            RGBABlend = a.Blend,
            LightColor = a.Color,
        };

        public static implicit operator MVertex(Vertex a) => new MVertex()
        {
            Position = a.InvPosition,
            Normal = a.RGBAInvNormal,
            UV0 = a.UV0,
            UV1 = a.UV1,
            Blend = a.RGBABlend,
            Color = a.LightColor,
        };

    }
}
