using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;

namespace SyneticLib.Utils;

public class WR2CobFileCreator
{
    public static void CreateCobFiles(string path)
    {
        var objectDir = Path.Combine(path, "Objects");
        var colliDir = Path.Combine(path, "Colli");
        Directory.CreateDirectory(colliDir);

        var sbNames = new StringBuilder();

        foreach (var moxFilePath in Directory.EnumerateFiles(objectDir))
        {
            if (Path.GetExtension(moxFilePath).ToLower() == ".mox")
            {
                var name = Path.GetFileNameWithoutExtension(moxFilePath);

                var cobFileName = Path.ChangeExtension(name, ".cob");
                var cobFilePath = Path.Combine(colliDir, cobFileName);

                var cpoFileName = Path.ChangeExtension(name, ".cpo");
                var cpoFilePath = Path.Combine(colliDir, cpoFileName);

                if (false && File.Exists(cpoFilePath))
                {
                    var cpo = new CpoFile();
                    cpo.Load(cpoFilePath);
                    var cob = CreateCobFile(cpo);
                    cob.Save(cobFilePath);
                }
                else
                {
                    var mox = new MoxFile();
                    mox.Load(moxFilePath);
                    var cob = CreateCobFile(mox);
                    cob.Save(cobFilePath);
                }

                sbNames.AppendLine(name);
            }
        }

        var namesFileName = Path.Combine(colliDir, "ColliList.txt");
        File.WriteAllText(namesFileName, sbNames.ToString());
    }

    public static CobFile CreateCobFile(MoxFile mox)
    {
        var cob = new CobFile();
        cob.Vertecis = mox.Vertecis;
        cob.Indices = mox.Indices;
        cob.Head.VerticeCount = cob.Vertecis.Length;
        cob.Head.PolyCount = cob.Indices.Length;
        cob.Head.BoundingBox = new BoundingBox(cob.Vertecis, cob.Indices);
        return cob;
    }

    public static CobFile CreateCobFile(CpoFile cpo)
    {
        var cob = new CobFile();
        return cob;
    }
}
