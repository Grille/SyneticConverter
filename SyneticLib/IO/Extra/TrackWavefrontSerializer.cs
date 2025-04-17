using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files.Extra;
using SyneticLib.IO.Generic;

namespace SyneticLib.IO.Extra;
public class TrackWavefrontSerializer : ISerializer<Track>
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

        for (var i = 0; i < nodes.Length; i++)
        {
            var v = nodes[i].Position;
            writer.WriteLine($"v {v.X} {v.Y} {v.Z}");
        }

        writer.Write("l");

        for (var i = 0; i < nodes.Length; i++)
        {
            writer.Write(' ');
            writer.Write(i + 1);
        }
    }

    IReadOnlyCollection<string> ISerializer<Track>.GetErrors()
    {
        throw new NotImplementedException();
    }
}
