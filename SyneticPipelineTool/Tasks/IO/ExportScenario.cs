using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.PipelineTool;

using SyneticLib;
using SyneticLib.IO;
using SyneticLib.IO.Generic;
using SyneticLib.Utils;
using SyneticLib.World;

namespace SyneticPipelineTool.Tasks.IO
{
    [PipelineTask("Synetic/Scenario.Export")]
    internal class ExportScenario : PipelineTask
    {
        static readonly Dictionary<string, DirectoryFileSerializer<Scenario>> _exporters;

        static readonly string[] _keys;

        static ExportScenario()
        {
            _exporters = new Dictionary<string, DirectoryFileSerializer<Scenario>>
            {
                { "Wavefront", Serializers.Scenario.Wavefront },
                { "BeamNG", Serializers.Scenario.BeamNG },
                { "Blender", Serializers.Scenario.Sbe },
            };

            _keys = _exporters.Keys.ToArray();
        }

        protected override void OnInit()
        {
            Parameters.Def(ParameterTypes.Object, "Scenario", "");
            Parameters.Def(ParameterTypes.SaveFile, "DstFile", "");
            Parameters.Def(ParameterTypes.Enum, "Exporter", "", _keys[0], _keys);
        }

        protected override void OnExecute()
        {
            var scn = EvalParameter("Scenario").GetAs<Scenario>();
            string key = EvalParameter("Exporter");

            string dstfile = EvalParameter("DstFile");

            Runtime.Logger.Info($"Export files {key} {dstfile}");
            Runtime.Logger.IncScope();

            _exporters[key].Save(dstfile, scn);

            Runtime.Logger.DecScope();
        }
    }
}
