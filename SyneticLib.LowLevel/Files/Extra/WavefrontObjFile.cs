using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

using SyneticLib.Files.Common;

namespace SyneticLib.Files.Extra;

public unsafe class WavefrontObjFile : TextFile, IVertexData, IIndexData
{
    public Vertex[] Vertecis { get; set; }

    public IdxTriangleInt32[] Indices { get; set; }

    public Section[]? Sections { get; set; }

    public bool IncludeColor = false;

    public WavefrontObjFile()
    {
        Vertecis = Array.Empty<Vertex>();
        Indices = Array.Empty<IdxTriangleInt32>();
    }

    public override void Deserialize(StreamReader reader)
    {
        var v = new List<Vector3>();
        var vc = new List<Vector3>();
        var vt = new List<Vector2>();
        var vn = new List<Vector3>();
        var f = new List<FaceIndices>();

        var sections = new List<Section>
        {
            new Section()
        };

        while (true)
        {
            var line = reader.ReadLine();

            if (line == null)
            {
                break;
            }

            var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (split.Length == 0)
            {
                continue;
            }

            switch (split[0])
            {
                case "v":
                {
                    ParseSingles(v, split.AsSpan(1, 3));
                    if (split.Length == 7)
                    {
                        ParseSingles(vc, split.AsSpan(4, 3));
                    }
                    break;
                }
                case "vt":
                {
                    ParseSingles(vt, split.AsSpan(1, 2));
                    break;
                }
                case "vn":
                {
                    ParseSingles(vn, split.AsSpan(1, 3));
                    break;
                }
                case "usemtl":
                {
                    break;
                }
                case "o":
                {
                    break;
                }
                case "l":
                {
                    throw new NotSupportedException();
                }
                case "f":
                {
                    ParseFace(f, split.AsSpan(1, 3));
                    break;
                }

            }
        }

        var vertecis = new List<Vertex>();
        var indices = new IdxTriangleInt32[f.Count / 3];
        var vtxdict = new Dictionary<Vertex, int>();

        fixed (void* vptr = indices)
        {
            int* iptr = (int*)vptr;
            int length = indices.Length * 3;
            for (int i = 0; i < length; i++)
            {
                var faceidx = f[i];
                var vertex = new Vertex()
                {
                    Position = v[faceidx.V],
                    Normal = vn[faceidx.V],
                    UV0 = vt[faceidx.V],
                };

                if (vtxdict.TryGetValue(vertex, out int index))
                {
                    iptr[i] = index;
                }
                else
                {
                    int newidx = vertecis.Count;
                    iptr[i] = newidx;
                    vtxdict.Add(vertex, newidx);
                    vertecis.Add(vertex);
                }
            }
        }

        Vertecis = vertecis.ToArray();
        Indices = indices;
    }

    private void ParseSingles<T>(List<T> list, ReadOnlySpan<string> span) where T : unmanaged
    {
        var obj = new T();
        var ptr = (float*)&obj;
        for (int i = 0; i < span.Length; i++)
        {
            float value = float.Parse(span[i]);
            ptr[i] = value;
        }

        list.Add(obj);
    }

    private void ParseFace(List<FaceIndices> list, ReadOnlySpan<string> span)
    {
        if (span.Length != 3)
        {
            throw new ArgumentException();
        }

        list.Add(ParseIndices(span[0]));
        list.Add(ParseIndices(span[1]));
        list.Add(ParseIndices(span[2]));
    }

    private FaceIndices ParseIndices(string text)
    {
        var split = text.Split('/');
        return new FaceIndices()
        {
            V = int.Parse(split[0]) - 1,
            VT = int.Parse(split[1]) - 1,
            VN = int.Parse(split[2]) - 1,
        };
    }

    public override void Serialize(StreamWriter writer)
    {
        void WritePrefix(string prefix)
        {
            writer.Write(prefix);
            WriteSpace();
        }

        void WriteSpace() => writer.Write(' ');

        void WriteLine() => writer.WriteLine();

        for (var i = 0; i < Vertecis.Length; i++)
        {
            ref var vertex = ref Vertecis[i];
            writer.WriteLine($"v {vertex.Position.X} {vertex.Position.Y} {vertex.Position.Z}");
        }

        for (var i = 0; i < Vertecis.Length; i++)
        {
            ref var vertex = ref Vertecis[i];
            writer.WriteLine($"vn {vertex.Normal.X} {vertex.Normal.Y} {vertex.Normal.Z}");
        }

        for (var i = 0; i < Vertecis.Length; i++)
        {
            ref var vertex = ref Vertecis[i];
            writer.WriteLine($"vt {vertex.UV0.X} {vertex.UV0.Y}");
        }

        void WriteIndex(int index)
        {
            writer.Write(index + 1);
            writer.Write('/');
            writer.Write(index + 1);
            writer.Write('/');
            writer.Write(index + 1);
        }

        for (var i = 0; i < Indices.Length; i++)
        {
            ref var index = ref Indices[i];
            WritePrefix("f");
            WriteIndex(index.X);
            WriteSpace();
            WriteIndex(index.Y);
            WriteSpace();
            WriteIndex(index.Z);
            WriteLine();
        }
    }

    public class Section
    {
        public string? Name;
        public int Start;
        public int Length;
    }

    

    private struct FaceIndices
    {
        public int V, VT, VN;
    }
}
