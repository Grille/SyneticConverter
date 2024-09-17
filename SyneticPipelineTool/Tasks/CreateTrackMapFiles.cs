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

[PipelineTask("Synetic/Create TrackMap Files")]
public class CreateTrackMapFiles : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.Directory, "Src Dir", null, "track");
        Parameters.Def(ParameterTypes.Directory, "Dst Dir", null, "maps");
        Parameters.Def(ParameterTypes.Enum, "Target", null, "WR2", new string[] { "WR2" });
    }

    protected override void OnExecute()
    {
        var srcDir = EvalParameter("Src Dir");
        var dstDir = EvalParameter("Dst Dir");

        int size = 512;
        int border = 50;

        Directory.CreateDirectory(dstDir);

        foreach (var file in Directory.EnumerateFiles(srcDir))
        {
            if (!Path.GetExtension(file).Equals(".trk", StringComparison.InvariantCultureIgnoreCase))
            {
                continue;
            }

            var name = Path.GetFileNameWithoutExtension(file);
            var dstName = $"Track{name.AsSpan(0, name.Length - 3)}{name.AsSpan(name.Length - 2, 2)}.tga";


            var track = Serializers.Track.Trk.Load(file);
            var texture = TrackMapGenerator.CreateTrackMap(track, size, size, border / (float)size);

            var dstpath = Path.Combine(dstDir, dstName);
            Serializers.Texture.Tga.Save(dstpath, texture);
        }
    }
}
