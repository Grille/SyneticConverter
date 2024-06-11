using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using Grille.IO;
using SyneticLib.Files.Common;

namespace SyneticLib.Files;
public class VtxFile : BinaryFile, IVertexData
{
    public int[] IndicesOffset { get; set; }
    public Vertex[] Vertecis { get; set; }

    public VtxFile()
    {
        IndicesOffset = Array.Empty<int>();
        Vertecis = Array.Empty<Vertex>();
    }

    public unsafe override void Deserialize(BinaryViewReader br)
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

    public override void Serialize(BinaryViewWriter bw)
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
            Position = a.Position,
            RGBANormal = a.Normal,
            UV0 = a.UV,
            UV1 = Vector2.Zero,
            RGBABlend = a.Blend,
            LightColor = a.Color.ToNormalizedRgbVector3(),
        };

        public static implicit operator MVertex(Vertex a) => new MVertex()
        {
            Position = a.Position,
            Normal = a.RGBANormal,
            UV = a.UV0,
            Blend = a.RGBABlend,
            Color = BgraColor.FromNormalizedRgbVector3(a.LightColor),
        };
    }
}


