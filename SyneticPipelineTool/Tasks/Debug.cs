using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.PipelineTool;

using SyneticLib.Files;
using SyneticLib.IO;
using OpenTK.Mathematics;

namespace SyneticPipelineTool.Tasks;

[PipelineTask("Synetic/DEBUG")]
public class Debug : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.Generic, "Arg0", null, null);
    }

    protected override void OnExecute()
    {
        var arg0 = EvalParameter("Arg0");

        var ro0 = new Ro0File();
        ro0.Load(Path.ChangeExtension(arg0, "ros"));

        for (int i = 0; i < ro0.Grass.Length; i++)
        {
            ref var gras = ref ro0.Grass[i];

            var gx = new Ro0File.ColorU16R4G4B4();

            gx.Encode(gras.Color0);
            gras.Color0 = gx.Decode();

            gx.Encode(gras.Color1);
            gras.Color1 = gx.Decode();

            //gras.Color0.X = 0;
            //gras.Color1.X = 1;
            //gras.Color0.Y = 0;
            //gras.Color1.Y = 1;
            //gras.Color0.Z = 0;
            //gras.Color1.Z = 1;

            ////              ___rg_bb_rg_bb
            //gras.Unknown0 = 0x_00_00_00_0fu;
            //              ___rrrr_rrgg__gggb_bbbb
            //gras.Unknown0 = 0b_0000_0000__0010_0000__0000_0000__0000_0000;
        }

        ro0.Save(arg0);
    }
}
