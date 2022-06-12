using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticConverter;
public abstract class MeshExporter
{
    protected Mesh target;
    protected string path;

    public MeshExporter(Mesh target)
    {
        this.target = target;
        //path = target.RootDir;
    }

    public abstract void Save();

    public void Save(string path)
    {
        this.path = path;
        Save();
    }
}
