using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;


using SyneticLib.LowLevel;

namespace SyneticLib;

public class Mesh: Ressource
{
    public Vertex[] Vertices { get; }
    public IndexTriangle[] Indices { get; }

    public Mesh(string path, Vertex[] vertices, IndexTriangle[] indices) : base(path)
    {
        Vertices = vertices;
        Indices = indices;
    }

    /*
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
    */

    public MeshSectionPtr CreateSectionPtr(int offset, int count)
    {
        return new MeshSectionPtr(this, offset, count);
    }
}
