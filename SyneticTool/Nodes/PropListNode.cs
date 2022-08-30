using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib;

namespace SyneticTool.Nodes;

public class PropListNode : DataListTreeNode<PropClass>
{
    public PropListNode(RessourceList<PropClass> list) : base(list, (a) => new PropNode(a))
    {
        SelectedImageIndex = ImageIndex = IconList.Misc;
    }

    protected override void OnUpdateAppearance()
    {
        base.OnUpdateAppearance();
        Text = $"Props [{DataValue.Count}]";
    }
}
