using SyneticLib.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticTool.Nodes;

public class PropNode : RessourceNode
{
    public new PropClass Value => (PropClass)base.Ressource;

    public PropNode(PropClass data) : base(data)
    {
        Nodes.Add($"{data.Name}");
    }

    protected override void OnUpdateAppearance()
    {
        base.OnUpdateAppearance();
        Text = $"{Value.Name}";
    }
}
