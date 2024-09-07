using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files.Extra;

namespace SyneticLib.IO;
public class TrackObjSerializer : ISerializer<Track>
{
    public Track Load(string path)
    {
        throw new NotImplementedException();
    }

    public void Save(string path, Track track)
    {
        var nodes = track.Nodes;

        using var stream = File.Create(path);
        using var writer = new StreamWriter(stream);

        writer.WriteLine("o track");

        for (int i = 0; i < nodes.Length; i++)
        {
            var v = nodes[i].Position;
            writer.WriteLine($"v {v.X} {v.Y} {v.Z}");
        }

        writer.Write("l");

        for (int i = 0; i < nodes.Length; i++)
        {
            writer.Write(' ');
            writer.Write(i + 1);
        }
    }
}
