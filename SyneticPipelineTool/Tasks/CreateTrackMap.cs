using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.PipelineTool;

using SyneticLib.IO;

namespace SyneticPipelineTool.Tasks;

[PipelineTask("Converter/Create TrackMaps")]
public class CreateTrackMaps : PipelineTask
{
    protected override void OnExecute()
    {
        string srcDir = EvalParameter("Src Directory");
        string dstDir = EvalParameter("Dst Directory");

        Directory.CreateDirectory(dstDir);

        foreach (var srcFile in Directory.GetFiles(srcDir))
        {
            if (Path.GetExtension(srcFile).ToLower() != ".trk")
                continue;

            var name = Path.GetFileNameWithoutExtension(srcFile);
            var dstName = $"Track{name.AsSpan(0, name.Length - 3)}{name.AsSpan(name.Length - 2, 2)}.tga";
            var dstFile = Path.Combine(dstDir, dstName);

            var track = Imports.LoadTrackFromTrk(srcFile);
            var texture = track.CreateTrackMap(512, 512);
            Exports.TextureAsTga(texture, dstFile);
        }
    }

    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Src Directory", "", ".trk");
        Parameters.Def(ParameterTypes.String, "Dst Directory", "", ".tga");
    }
}
