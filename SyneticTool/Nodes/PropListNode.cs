using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib;
using SyneticLib.World;
using SyneticTool.Nodes.System;

namespace SyneticTool.Nodes;

public class PropListNode : RessourceListNode<PropClass>
{
    public PropListNode(RessourceList<PropClass> list) : base(list, (a) => new PropNode(a))
    {
        SelectedImageIndex = ImageIndex = IconList.Misc;
    }

    protected override void OnUpdateAppearance()
    {
        base.OnUpdateAppearance();
        Text = $"Props [{Value.Count}]";
    }
}
