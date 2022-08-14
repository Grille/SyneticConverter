using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using GGL.IO;

namespace SyneticLib.IO.Synetic.Files;
public class VtxFile : SyneticBinaryFile, IVertexData
{
    public int[] VtxQty { get; set; }
    public MeshVertex[] Vertices { get; set; }

    public unsafe override void ReadFromView(BinaryViewReader br)
    {
        VtxQty = br.ReadArray<int>(64);

        var vertexCount = 0;
        for (var i = 0; i < VtxQty.Length; i++)
            vertexCount += VtxQty[i];

        var finalpos = br.Position + vertexCount * sizeof(MVertex);
        if (br.Length != finalpos)
            throw new InvalidOperationException($"{br.Length} != {finalpos}");

        Vertices = new MeshVertex[vertexCount];
        for (var i = 0; i < vertexCount; i++)
        {
            var src = br.Read<MVertex>();
            Vertices[i] = new MeshVertex()
            {
                Position = src.Position,
                Normal = new Vector4(src.Normal.B / 255f, src.Normal.G / 255f, src.Normal.R / 255f, src.Normal.A / 255f),
                UV0 = src.UV,
                UV1 = Vector2.Zero,
                Blending = new Vector3(src.Blend.B0, src.Blend.B1, src.Blend.B2),
                Shadow = src.Blend.Shadow,
                LightColor = src.Color,
            };
        }
    }

    public override void WriteToView(BinaryViewWriter bw)
    {
        bw.WriteArray(VtxQty, LengthPrefix.None);

        for (var i = 0; i < Vertices.Length; i++)
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


