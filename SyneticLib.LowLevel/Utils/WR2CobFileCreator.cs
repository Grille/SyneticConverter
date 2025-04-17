using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;
using SyneticLib.Files.Common;
using SyneticLib.Files.Extra;

namespace SyneticLib.Utils;

public class WR2CobFileCreator
{
    public static void CreateCobFiles(string path)
    {
        CreateCobFiles(path, null);
    }

    public static void CreateCobFiles(string path, Predicate<string>? filter)
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

                if (filter != null && !filter(name))
                {
                    continue;
                }

                var cobFileName = Path.ChangeExtension(name, ".cob");
                var cobFilePath = Path.Combine(colliDir, cobFileName);

                var cpoFileName = Path.ChangeExtension(name, ".cpo");
                var cpoFilePath = Path.Combine(colliDir, cpoFileName);

                var objFileName = Path.ChangeExtension(name, ".obj");
                var objFilePath = Path.Combine(colliDir, objFileName);

                if (File.Exists(objFilePath))
                {
                    var obj = new WavefrontObjFile();
                    obj.Load(objFilePath);
                    var cob = CreateCobFile(obj, obj);
                    cob.Save(cobFilePath);
                    File.Delete(objFilePath);
                }
                else if (false && File.Exists(cpoFilePath))
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
        return CreateCobFile(mox, mox);
    }

    public static CobFile CreateCobFile(IVertexData vtx, IIndexData idx)
    {
        var cob = new CobFile();
        cob.Vertecis = vtx.Vertecis;
        cob.Triangles = idx.Triangles;
        cob.Head.VerticeCount = cob.Vertecis.Length;
        cob.Head.PolyCount = cob.Triangles.Length;
        cob.Head.BoundingBox = new BoundingBox(cob.Vertecis, cob.Triangles);
        return cob;
    }

    public static CobFile CreateCobFile(CpoFile cpo)
    {
        var cob = new CobFile();
        return cob;
    }
}
