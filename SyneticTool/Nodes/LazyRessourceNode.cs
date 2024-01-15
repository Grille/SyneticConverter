using SyneticLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticTool.Nodes;

public class LazyRessourceNode<T> : BaseNode where T : SyneticObject
{
    public new Lazy<T> Value => (Lazy<T>)base.Value;

    public LazyRessourceNode(Lazy<T> obj) : base(obj)
    {
    }
}
