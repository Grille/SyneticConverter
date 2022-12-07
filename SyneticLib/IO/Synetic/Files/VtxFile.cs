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
    public int[] IndicesOffset { get; set; }
    public Vertex[] Vertecis { get; set; }

    public unsafe override void ReadFromView(BinaryViewReader br)
    {
        IndicesOffset = br.ReadArray<int>(64);

        var vertexCount = 0;
        for (var i = 0; i < IndicesOffset.Length; i++)
            vertexCount += IndicesOffset[i];

        var finalpos = br.Position + vertexCount * sizeof(MVertex);
        if (br.Length != finalpos)
            throw new InvalidOperationException($"{br.Length} != {finalpos}");

        Vertecis = new Vertex[vertexCount];
        for (var i = 0; i < vertexCount; i++)
            Vertecis[i] = br.Read<MVertex>();
    }

    public override void WriteToView(BinaryViewWriter bw)
    {
        bw.WriteArray(IndicesOffset, LengthPrefix.None);

        for (var i = 0; i < Vertecis.Length; i++)
            bw.Write<MVertex>(Vertecis[i]);
    }

    public struct MVertex
    {
        public Vector3 Position;
        public BgraColor Normal;
        public Vector2 UV;
        public BlendColor Blend;
        public BgraColor Color;

        public static implicit operator Vertex(MVertex a) => new Vertex()
        {
            InvPosition = a.Position,
            RGBAInvNormal = a.Normal,
            UV0 = a.UV,
            UV1 = Vector2.Zero,
            RGBABlend = a.Blend,
            LightColor = a.Color,
        };

        public static implicit operator MVertex(Vertex a) => new MVertex()
        {
            Position = new Vector3(a.Position.X, a.Position.Z, a.Position.Y),
            Normal = a.RGBAInvNormal,
            UV = a.UV0,
            Blend = a.RGBABlend,
            Color = a.LightColor,
        };
    }
}


