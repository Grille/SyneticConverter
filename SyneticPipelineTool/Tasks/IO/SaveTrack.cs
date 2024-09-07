using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.PipelineTool;
using Grille.PipelineTool.IO;

using SyneticLib;
using SyneticLib.IO;

namespace SyneticPipelineTool.Tasks.IO;

[PipelineTask("Synetic/IO/Track.Save")]
internal class SaveTrack : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.SaveFile, "Dst File", "Save track to file", "./track.trk");
        Parameters.Def(ParameterTypes.Enum, "File Type", null, SerializerTaskUtils.Default, new string[] { SerializerTaskUtils.Auto, ".TRK", ".OBJ"});
        Parameters.Def(ParameterTypes.Object, "Track", null, "*Track", null);
    }

    protected override void OnExecute()
    {
        var path = EvalParameter("Dst File");
        var type = EvalParameter("File Type");
        var texture = EvalParameter("Track");

        SerializerTaskUtils.SaveTrack(path, type, texture);
    }

    public override Token[] ToTokens() => new Token[]
    {
        new(TokenType.Text, "Track.Save "),
        new(TokenType.Expression, Parameters["Track"]),
        new(TokenType.Text, " in "),
        new(TokenType.Expression, Parameters["Dst File"]),
    };
}
