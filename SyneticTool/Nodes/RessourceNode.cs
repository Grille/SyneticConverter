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
    public SyneticObject Ressource => (SyneticObject)Value;

    public RessourceNode(SyneticObject data) : base(data)
    {

    }
}
