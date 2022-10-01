using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class MeshSectionPtr
{
    public readonly Mesh Target;
    public int Offset;
    public int Count;

    public MeshSectionPtr(Mesh target, int offset, int count)
    {
        Target = target;
    }
}
