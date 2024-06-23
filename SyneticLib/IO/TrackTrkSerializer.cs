using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;

namespace SyneticLib.IO;
public class TrackTrkSerializer : FileSerializer<TrkFile, Track>
{
    public override Track OnDeserialize(TrkFile trk)
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

    protected override void OnSerialize(TrkFile file, Track value)
    {
        throw new NotImplementedException();
    }
}
