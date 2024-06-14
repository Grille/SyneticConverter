using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SyneticLib.Files;

namespace SyneticLib.IO;

public static partial class Imports
{
    public static Track LoadTrackFromTrk(string path) 
    { 
        var trk = new TrkFile();
        trk.Load(path);
        return LoadTrackFromTrk(trk);
    }

    public static Track LoadTrackFromTrk(TrkFile trk)
    {
        var nodes = new TrackNode[trk.Head.Nodes];

        for (int i = 0; i < nodes.Length; i++)
        {
            var node = new TrackNode();
            node.Position = trk.Nodes[i].Position;
            nodes[i] = node;
        }

        return new Track(nodes);
    }
}
