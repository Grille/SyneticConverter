using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.PipelineTool;

using SyneticLib.Files;
using SyneticLib.IO;
using SyneticLib.Utils;

namespace SyneticPipelineTool.Tasks;

[PipelineTask("Synetic/Create SC2-AddonScenario File")]
public class CreateSC2 : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "File Name", null, "ALPS");
        Parameters.Def(ParameterTypes.String, "Display Name", null, "Alps");
        Parameters.Def(ParameterTypes.Directory, "Tracks Directory", null, "./Tracks");
        Parameters.Def(ParameterTypes.SaveFile, "Output File", null, "./EditScenery.sc2");
    }

    protected override void OnExecute()
    {
        var fileName = EvalParameter("File Name");
        var displayName = EvalParameter("Display Name");
        var tracksDir = EvalParameter("Tracks Directory");
        var output = EvalParameter("Output File");

        var list = new List<Sc2File.Track>();

        ushort id = 0;

        foreach (var file in Directory.EnumerateFiles(tracksDir))
        {
            if (!Path.GetExtension(file).Equals(".trk", StringComparison.InvariantCultureIgnoreCase))
            {
                continue;
            }

            id += 1;

            var track = Serializers.Track.Trk.Load(file);
            var trackName = Path.GetFileNameWithoutExtension(file);
            var trackMapFileName = $"Track_{trackName}.tga";

            var scTrack = new Sc2File.Track();

            scTrack.ID = id;
            scTrack.Direction = 1;
            scTrack.Name = trackName;
            scTrack.MapImage = trackMapFileName;
            scTrack.Distance = (ushort)track.GetAbsDistance();

            list.Add(scTrack);
        }

        var sc2 = new Sc2File();

        sc2.FileName = fileName;
        sc2.DisplayName = displayName;
        sc2.BackgroundImage = $"BG_{fileName}.tga";
        sc2.FlagImage = $"Flag_{fileName}.tga";
        sc2.Tracks = list.ToArray();

        sc2.Save(output);
    }
}
