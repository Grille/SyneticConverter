using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

using SyneticLib.Files.Common;

namespace SyneticLib.Files.Extra;

public class WavefrontObjFile : TextFile, IVertexData, IIndexData
{
    public Vertex[] Vertecis { get; set; }

    public IdxTriangleInt32[] Indices { get; set; }

    public bool IncludeColor = false;

    public WavefrontObjFile()
    {
        Vertecis = Array.Empty<Vertex>();
        Indices = Array.Empty<IdxTriangleInt32>();
    }

    public override void Deserialize(StreamReader reader)
    {
        var v = new List<Vector3>();
        var vt = new List<Vector2>();
        var vn = new List<Vector3>();

        while (true)
        {
            var line = reader.ReadLine();
            if (line == null)
                break;

            var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (split.Length == 0)
                continue;

            switch (split[0])
            {
                case "v":
                {

                    break;
                }
                case "vt":
                {
                    break;
                }
                case "vn":
                {
                    break;
                }
                case "f":
                {
                    break;
                }

            }
        }
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
            writer.Write(index);
            writer.Write('/');
            writer.Write(index);
            writer.Write('/');
            writer.Write(index);
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
}
