using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;

namespace SyneticLib.IO;
public class ModelSyneticSerializer : ISerializer<Model>
{
    public Model Load(string path)
    {
        var moxPath = Path.ChangeExtension(path, "mox");
        var mtlPath = Path.ChangeExtension(path, "mtl");

        var mox = new MoxFile();
        var mtl = new MtlFile();

        mox.Load(moxPath);
        mtl.Load(mtlPath);

        return null;
    }

    public void Save(string path, Model obj)
    {
        throw new NotImplementedException();
    }
}
