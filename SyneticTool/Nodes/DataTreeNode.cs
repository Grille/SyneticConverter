using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SyneticLib;

namespace SyneticTool.Nodes;

public class DataTreeNode : MyTreeNode
{
    public Ressource Ressource => (Ressource)Object;

    public DataTreeNode(Ressource data) : base(data)
    {

    }
}
