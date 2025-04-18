﻿using System;
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

    public IdxTriangleInt32[] Triangles { get; set; }

    public Section[]? Sections { get; set; }

    public ObjModel[]? Models { get; set; }

    public bool IncludeColor = false;

    public WavefrontObjFile()
    {
        Vertecis = Array.Empty<Vertex>();
        Triangles = Array.Empty<IdxTriangleInt32>();
    }

    public override void Deserialize(StreamReader reader)
    {
        var v = new List<Vector3>();
        var vc = new List<Vector3>();
        var vt = new List<Vector2>();
        var vn = new List<Vector3>();
        var f = new List<FaceIndices>();

        var sections = new List<Section>();

        var section = new Section("default", 0);

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
                    ParsePath(section, split.AsSpan(1), v);
                    break;
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
                    Normal = vn[faceidx.VN],
                    UV0 = vt[faceidx.VT],
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
        Triangles = indices;
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
            V = ParseIndex(split[0]),
            VT = ParseIndex(split[1]),
            VN = ParseIndex(split[2]),
        };
    }

    private void ParsePath(Section section, ReadOnlySpan<string> span, List<Vector3> v)
    {
        if (section.Path != null)
        {
            throw new ArgumentException();
        }

        var path = new Vector3[span.Length];

        for (int i = 0; i <= span.Length; i++)
        {
            int idx = ParseIndex(span[i]);
            path[i] = v[idx];
        }

        section.Path = path;
    }

    private int ParseIndex(string text)
    {
        return int.Parse(text) - 1;
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
            writer.WriteLine($"vt {vertex.UV0.X} {1-vertex.UV0.Y}");
        }

        void WriteIndex(int index)
        {
            writer.Write(index + 1);
            writer.Write('/');
            writer.Write(index + 1);
            writer.Write('/');
            writer.Write(index + 1);
        }

        void WriteFace(IdxTriangleInt32 index)
        {
            WritePrefix("f");
            WriteIndex(index.X);
            WriteSpace();
            WriteIndex(index.Y);
            WriteSpace();
            WriteIndex(index.Z);
            WriteLine();
        }

        void WriteCmd(string command, string value)
        {
            WritePrefix(command);
            writer.Write(value);
            WriteLine();
        }

        if (Models == null)
        {
            for (var i = 0; i < Triangles.Length; i++)
            {
                WriteFace(Triangles[i]);
            }
        }
        else
        {
            foreach (var model in Models)
            {
                WriteCmd("o", model.Name);
                foreach (var region in model.Regions)
                {
                    WriteCmd("usemtl", region.Material);
                    for (int i = 0; i < region.Length; i++)
                    {
                        WriteFace(Triangles[i + region.Offset]);
                    }
                }
            }
        }
    }

    public class Section
    {
        public string Name;
        public Vector3[]? Path;
        public int Start;
        public int Length;

        public Section(string name, int start)
        {
            Name = name;
            Start = start;
        }
    }

    public record class ObjModel(string Name, MtlRegion[] Regions);

    public struct MtlRegion
    {
        public string Material;
        public int Offset;
        public int Length;
    }



    private struct FaceIndices
    {
        public int V, VT, VN;
    }
}
