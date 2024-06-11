using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;

namespace SyneticLib;

public class Mesh: SyneticObject
{
    public Vertex[] Vertices { get; }
    public IdxTriangleInt32[] Indices { get; }

    public Mesh(Vertex[] vertices, IdxTriangleInt32[] indices) 
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

    public MeshSegment CreateSectionPtr(int offset, int count)
    {
        return new MeshSegment(this, offset, count);
    }
}
