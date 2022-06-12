using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticConverter;
public abstract class MeshImporter
{
    protected Mesh target;
    protected string path;

    public MeshImporter(Mesh target)
    {
        this.target = target;
    }

    public abstract void Load();
    public abstract void Assign();

    public void Load(string path)
    {
        this.path = path;
        Load();
    }

    public void LoadAndAssign()
    {
        Load();
        Assign();
    }
}
