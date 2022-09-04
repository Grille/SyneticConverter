using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.IO;
public abstract class MeshImporter
{
    protected Mesh Target { get; set; }

    public MeshImporter(Mesh target)
    {
        this.Target = target;
    }

    protected abstract void OnLoad();
    protected abstract void OnAssign();

    public void Load()
    {
        OnLoad();
        OnAssign();
    }
}
