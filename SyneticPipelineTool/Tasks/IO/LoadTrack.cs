using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

using Grille.PipelineTool;
using Grille.PipelineTool.IO;

using SyneticLib.IO;

namespace SyneticPipelineTool.Tasks.IO;

[PipelineTask("Synetic/IO/Track.Load")]
internal class LoadTrack : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.OpenFile, "Src File", "Load track from file", "./track.trk");
        Parameters.Def(ParameterTypes.Enum, "File Type", null, ".TRK", new string[] { ".TRK" });
        Parameters.DefResult("Track");
    }

    protected override void OnExecute()
    {
        var path = EvalParameter("Src File");
        var type = EvalParameter("File Type");
        var texturename = EvalParameter("Track");

        Runtime.Variables[texturename] = SerializerTaskUtils.LoadTrack(path, type);
    }

    public override Token[] ToTokens() => new Token[]
    {
        new(TokenType.Text, "Track.Load "),
        new(TokenType.Expression, Parameters["Src File"]),
        new(TokenType.Text, " as "),
        new(TokenType.Expression, Parameters["Track"]),
    };
}
