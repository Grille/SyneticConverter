using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.PipelineTool;

using SyneticLib;
using SyneticLib.IO;
using SyneticLib.Utils;

using SyneticPipelineTool.Tasks.IO;

namespace SyneticPipelineTool.Tasks;

[PipelineTask("Synetic/Create TrackMap Texture")]
public class CreateTrackMapTexture : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.Generic, "Source", "Track file (.trk) or variable. ", "track.trk");
        Parameters.Def(ParameterTypes.Integer, "Size", null, "512");
        Parameters.Def(ParameterTypes.Integer, "Border", null, "32");
        Parameters.DefResult("Texture");
    }

    protected override void OnExecute()
    {
        var trackSource = EvalParameter("Source");
        var resultname = EvalParameter("Texture");

        int size = int.Parse(EvalParameter("Size"));
        int border = int.Parse(EvalParameter("Border"));

        var track = SerializerTaskUtils.GetValue(trackSource, Serializers.Track.Registry);
        var texture = TrackMapGenerator.CreateTrackMap(track, size, size, border / (float)size);

        Runtime.Variables[resultname] = new VariableValue(texture);
    }
}
