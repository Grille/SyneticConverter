using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.IO;

using OpenTK.Mathematics;

using SyneticLib.Files.Common;

namespace SyneticLib.Files;

public class CpoFile : BinaryFile, IVertexData, IIndexData
{
    public const int Magic = 0x43504F21;
    public MHead Head;

    public int[] IndicesOffset { get; set; }

    public Vertex[] Vertecis { get; set; }

    public IdxTriangleInt32[] Indices { get; set; }

    public CpoFile()
    {
        IndicesOffset = new int[1] { 0 };
        Vertecis = Array.Empty<Vertex>();
        Indices = Array.Empty<IdxTriangleInt32>();
    }

    public override void Deserialize(BinaryViewReader br)
    {
        Head = br.Read<MHead>();

        var positions = br.ReadArray<Vector3>(Head.VerticeCount);    
        var indices = br.ReadArray<IdxTriangleUInt16>(Head.PolyCount);
        var normals = br.ReadArray<Vector3>(Head.VerticeCount);

        Vertecis = new Vertex[Head.VerticeCount];
        for (int i = 0; i < Head.VerticeCount; i++)
        {
            Vertecis[i] = new Vertex()
            {
                Position = positions[i],
                Normal = normals[i],
            };
        }

        Indices = new IdxTriangleInt32[Head.PolyCount];
        for (int i = 0; i < Head.PolyCount; i++){
            Indices[i] = indices[i];
        }
    }

    public override void Serialize(BinaryViewWriter bw)
    {
        bw.Write(Head);

        for (int i = 0; i < Head.VerticeCount; i++)
        {
            bw.Write(Vertecis[i].Position);
        }

        for (int i = 0; i < Head.PolyCount; i++)
        {
            bw.Write((IdxTriangleUInt16)Indices[i]);
        }

        for (int i = 0; i < Head.VerticeCount; i++)
        {
            bw.Write(Vertecis[i].Normal);
        }
    }

    public struct MHead
    {
        public int VerticeCount;
        public int PolyCount;
        public Vector3 Center;
        public Pair X;
        public Pair Y;
        public Pair Z;

        public struct Pair
        {
            public float Min;
            public float Max;
        }

        public Vector3 Max
        {
            get => new Vector3(X.Max, Y.Max, Z.Max);
            set
            {
                X.Max = value.X; 
                Y.Max = value.Y; 
                Z.Max = value.Z;
            }
        }

        public Vector3 Min
        {
            get => new Vector3(X.Min, Y.Min, Z.Min);
            set
            {
                X.Min = value.X;
                Y.Min = value.Y;
                Z.Min = value.Z;
            }
        }

        public BoundingBox BoundingBox
        {
            get => new BoundingBox(Min, Max);
            set
            {
                Min = value.Start; 
                Max = value.End;
            }
        }
    }
}
