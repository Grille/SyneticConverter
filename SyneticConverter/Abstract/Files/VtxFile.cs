using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using GGL.IO;

namespace SyneticConverter;
public class VtxFile : SyneticBinFile, IVertexData
{
    public int[] VtxQty { get; set; }
    public Vertex[] Vertices { get; set; }

    public unsafe override void Read(BinaryViewReader br)
    {
        VtxQty = br.ReadArray<int>(64);

        int vertexCount = 0;
        for (int i = 0; i < VtxQty.Length; i++)
            vertexCount += VtxQty[i];

        long finalpos = br.Position + vertexCount * sizeof(MVertex);
        if (br.Length != finalpos)
            throw new InvalidOperationException($"{br.Length} != {finalpos}");

        Vertices = new Vertex[vertexCount];
        for (int i = 0; i < vertexCount; i++)
        {
            var src = br.Read<MVertex>();
            Vertices[i] = new Vertex()
            {
                Position = src.Position,
                Normal = new Vector4(src.Normal.B / 255f, src.Normal.G / 255f, src.Normal.R / 255f, src.Normal.A / 255f),
                UV0 = src.UV,
                UV1 = Vector2.Zero,
                Blending = new Vector3(src.Blend.B, src.Blend.G, src.Blend.R),
                Shadow = src.Blend.Shadow,
                Color = src.Color,
            };
        }
    }

    public override void Write(BinaryViewWriter bw)
    {
        bw.WriteArray(VtxQty, LengthPrefix.None);

        for (int i = 0; i < Vertices.Length; i++)
        {
            var src = Vertices[i];
            bw.Write<MVertex>(new()
            {
                Position = src.Position,
            });
        }
}

    public struct MVertex
    {
        public Vector3 Position;
        public BgraColor Normal;
        public Vector2 UV;
        public BlendColor Blend;
        public BgraColor Color;
    }
}


