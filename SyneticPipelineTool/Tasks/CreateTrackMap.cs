using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.PipelineTool;

using SyneticLib.IO;
using SyneticLib.Utils;

namespace SyneticPipelineTool.Tasks;

[PipelineTask("Synetic/Create TrackMap")]
public class CreateTrackMaps : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.OpenFile, "Src File", "", ".trk");
        Parameters.Def(ParameterTypes.Integer, "Size", null, "512");
        Parameters.DefResult("Texture");
    }

    protected override void OnExecute()
    {
        string srcFile = EvalParameter("Src File");
        string resultname = EvalParameter("Texture");

        var track = Serializers.Track.Trk.Load(srcFile);
        var texture = TrackMapGenerator.CreateTrackMap(track, 512, 512);

        Runtime.Variables[resultname] = new VariableValue(texture);
    }
}
