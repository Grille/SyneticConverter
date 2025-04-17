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
public class GeoFile : BinaryFile, IIndexData, IVertexData, IIndexDataOffsets
{
    public MHead Head;

    public int[] IndicesOffset { get; set; }

    public Vertex[] Vertecis { get; set; }

    public IdxTriangleInt32[] Triangles { get; set; }

    public GeoFile()
    {
        IndicesOffset = Array.Empty<int>();
        Vertecis = Array.Empty<Vertex>();
        Triangles = Array.Empty<IdxTriangleInt32>();
    }

    public unsafe override void Deserialize(BinaryViewReader br)
    {
        Head = br.Read<MHead>();

        IndicesOffset = br.ReadArray<int>(Head.IndicesOffsetCount);

        int vertexCount = GetVertexCount();
        AssertFileSize(vertexCount, Head.IndicesCount, (int)br.Length);

        Vertecis = new Vertex[vertexCount];
        for (var i = 0; i < vertexCount; i++)
            Vertecis[i] = br.Read<MVertex>();

        if (Head.SectionCount == 3)
            br.ReadArray<byte>(vertexCount * 16);

        var indices = br.ReadArray<ushort>(Head.IndicesCount);

        Triangles = new IdxTriangleInt32[indices.Length / 3];

        for (int i = 0; i < Triangles.Length; i++)
            Triangles[i] = new IdxTriangleInt32(indices[i * 3 + 0], indices[i * 3 + 1], indices[i * 3 + 2]);
    }

    public override void Serialize(BinaryViewWriter bw)
    {
        bw.Write(Head);

        bw.WriteArray(IndicesOffset, LengthPrefix.None);

        var vertexCount = GetVertexCount();

        for (int i = 0; i < vertexCount; i++)
            bw.Write<MVertex>(Vertecis[i]);

        if (Head.SectionCount == 3)
            bw.Seek(vertexCount * 16, SeekOrigin.Current);

        var indices = new ushort[Triangles.Length * 3];

        for (int i = 0; i < Triangles.Length; i++)
        {
            indices[i * 3 + 0] = (ushort)Triangles[i].X;
            indices[i * 3 + 1] = (ushort)Triangles[i].Y;
            indices[i * 3 + 2] = (ushort)Triangles[i].Z;
        }

        bw.WriteArray(indices, LengthPrefix.None);
    }

    public int GetVertexCount() => IVertexDataExtension.GetVertexCount(this, this);

    private unsafe int CalcFileSize(int vtxCount, int idxCount)
    {
        int endPos = 0;
        
        endPos += sizeof(MHead);
        endPos += sizeof(int) * 64;
        endPos += sizeof(MVertex) * vtxCount;

        if (Head.SectionCount == 3)
            endPos += vtxCount * 16;

        endPos += sizeof(ushort) * idxCount;

        return endPos;
    }

    private void AssertFileSize(int vertexCount, int IndicesCount, int length)
    {
        int calculatedEndPos = CalcFileSize(vertexCount, IndicesCount);

        if (calculatedEndPos != length)
            throw new Exception($"Invalid File Size: ({calculatedEndPos} != {length}) Diff {calculatedEndPos - length}");
    }

    public void FillHead()
    {
        Head.Magic = (String4)"MOEG";
        Head.X0 = 1;
        Head.X1 = 1;
        Head.SectionCount = 2;
        Head.IndicesOffsetCount = IndicesOffset.Length;
        Head.IndicesCount = Triangles.Length * 3;
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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct MVertex
    {
        public Vector3 Position;
        public BgraColor Normal;
        public Vector2 UV0;
        public Vector2 UV1;
        public BlendColor Blend;
        public RgbColor Color;
        public byte Unknown0;

        public static implicit operator Vertex(MVertex a) => new Vertex()
        {
            Position = a.Position,
            RGBANormal = a.Normal,
            UV0 = a.UV0,
            UV1 = a.UV1,
            RGBABlend = a.Blend,
            LightColor = a.Color.ToNormalizedRgbVector3(),
            Unknown0 = a.Unknown0,
        };

        public static implicit operator MVertex(Vertex a) => new MVertex()
        {
            Position = a.Position,
            Normal = a.RGBANormal,
            UV0 = a.UV0,
            UV1 = a.UV1,
            Blend = a.RGBABlend,
            Color = RgbColor.FromNormalizedRgbVector3(a.LightColor),
            Unknown0 = (byte)a.Unknown0,
        };

    }
}
