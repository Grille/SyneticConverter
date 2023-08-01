using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;

using SyneticLib.IO.Synetic;
using SyneticLib.IO.Extern;
using SyneticLib.LowLevel;

namespace SyneticLib;

public class Mesh: Ressource
{
    public string Name;
    //public int[] Indecies;
    public Vertex[] Vertices { get; init; }
    public IndexTriangle[] Indices { get; init; }

    public Mesh(Ressource parent, string path, PointerType type) : base(parent, path, type)
    {
    }

    public virtual void ExportAsObj(string path)
    {
        var exp = new MeshExporterObj(this);
        exp.Save(path);
    }

    public void ExportAsSbx(string path)
    {
        var exp = new MeshExporterSbx(this);
        exp.Save(path);
    }

    public MeshSectionPtr CreateSectionPtr(int offset, int count)
    {
        return new MeshSectionPtr(this, offset, count);
    }


}
