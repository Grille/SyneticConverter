using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SyneticLib;

namespace SyneticTool.Nodes;

public class RessourceNode : BaseNode
{
    public Ressource Ressource => (Ressource)Value;

    public RessourceNode(Ressource data) : base(data)
    {

    }
}
