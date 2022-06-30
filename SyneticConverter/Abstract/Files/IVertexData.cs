using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticConverter;
public interface IVertexData
{
    public int[] VtxQty { get; set; }
    public Vertex[] Vertices { get; set; }
}
