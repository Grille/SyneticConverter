using Grille.PipelineTool;
using SyneticLib.Conversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool.Tasks;

[PipelineTask("Converter/Rename Files")]
internal class RenameFiles : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.String, "Dir", "", "Dir");
        Parameters.Def(ParameterTypes.String, "Old Name", "", "Scenario");
        Parameters.Def(ParameterTypes.String, "New Name", "", "Scenario");
    }

    protected override void OnExecute()
    {
        string dirPath = EvalParameter("Dir");
        var oldname = EvalParameter("Old Name");
        var newname = EvalParameter("New Name");

        var culture = CultureInfo.CurrentCulture;

        foreach (var file in Directory.EnumerateFiles(dirPath))
        {
            var filename = Path.GetFileName(file);
            if (filename.StartsWith(oldname, true, culture))
            {
                var dstname = filename.Replace(oldname, newname, true, culture);
                var dstpath = Path.Combine( Path.GetDirectoryName(file), dstname);
                File.Move(file, dstpath);
            }
        }
    }

}
