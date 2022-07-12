using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using GGL.IO;

namespace SyneticLib.IO.Synetic.Files;
public class GeoFile : SyneticBinFile, IIndexData, IVertexData
{
    public bool HasX16VertexBlock = false;

    public MHead Head;

    public int[] VtxQty { get; set; }
    public Vertex[] Vertices { get; set; }
    public ushort[] Indices { get; set; }

    public unsafe override void ReadFromView(BinaryViewReader br)
    {
        Head = br.Read<MHead>();

        VtxQty = br.ReadArray<int>(64);

        var vertexCount = 0;
        for (var i = 0; i < VtxQty.Length; i++)
            vertexCount += VtxQty[i];

        var calculatedEndPos = br.Position + vertexCount * sizeof(MVertex) + Head.IndicesCount * sizeof(ushort);
        if (HasX16VertexBlock)
            calculatedEndPos += vertexCount * 16;

        if (calculatedEndPos != br.Length)
            throw new Exception($"{calculatedEndPos} != {br.Length} diff:{calculatedEndPos - br.Length}");

        Vertices = new Vertex[vertexCount];
        for (var i = 0; i < vertexCount; i++)
        {
            var src = br.Read<MVertex>();
            Vertices[i] = new Vertex()
            {
                Position = src.Position,
                Normal = new Vector4(src.Normal.B / 255f, src.Normal.G / 255f, src.Normal.R / 255f, src.Normal.A / 255f),
                UV0 = src.UV0,
                UV1 = src.UV1,
                Blending = new Vector3(src.Blend.B0, src.Blend.B1, src.Blend.B2),
                Shadow = src.Blend.Shadow,
                LightColor = src.Color,
            };
        }

        if (HasX16VertexBlock)
            br.ReadArray<byte>(vertexCount * 16);

        Indices = br.ReadArray<ushort>(Head.IndicesCount);
    }

    public override void WriteToView(BinaryViewWriter bw)
    {
        throw new NotImplementedException();
    }

    public struct MHead
    {
        public String4 Magic;
        public int MaxSectionRange, X3, SectionCount, IndicesCount, XZ, Qty, Density;
    }

    private struct MVertex
    {
        public Vector3 Position;
        public BgraColor Normal;
        public Vector2 UV0;
        public Vector2 UV1;
        public BlendColor Blend;
        public BgraColor Color;
    }
}
