using Grille.PipelineTool;
using SyneticLib.Conversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool.Tasks.IO;

[PipelineTask("Synetic/IO/Rename Files")]
internal class RenameFiles : PipelineTask
{
    protected override void OnInit()
    {
        Parameters.Def(ParameterTypes.Directory, "Path", "Files inside directory or single file.", "Path");
        Parameters.Def(ParameterTypes.String, "Find", "", "Alps");
        Parameters.Def(ParameterTypes.String, "Replace", "", "MBWR_Alps");
    }

    protected override void OnExecute()
    {
        string path = EvalParameter("Path");
        var find = EvalParameter("Find");
        var replace = EvalParameter("Replace");

        void Move(string file)
        {
            var culture = CultureInfo.CurrentCulture;
            var filename = Path.GetFileName(file);
            if (filename.StartsWith(find, true, culture))
            {
                var dstname = filename.Replace(find, replace, true, culture);
                var dstpath = Path.Combine(Path.GetDirectoryName(file), dstname);
                File.Move(file, dstpath);
            }
        }

        if (File.Exists(path))
        {
            Move(path);
        }
        else if (Directory.Exists(path))
        {
            foreach (var file in Directory.EnumerateFiles(path))
            {
                Move(file);
            }
        }
        else
        {
            throw new FileNotFoundException(path);
        }
    }
}
