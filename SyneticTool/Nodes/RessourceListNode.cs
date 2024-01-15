using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib;

namespace SyneticTool.Nodes;

public class RessourceListNode<T> : BaseNode where T : SyneticObject
{
    public new RessourceList<T> Value => (RessourceList<T>)base.Value;

    public Func<T, RessourceNode> Constructor;

    public RessourceListNode(RessourceList<T> list, Func<T, RessourceNode> constructor) :base(list)
    {
        Constructor = constructor;
    }

    protected override void OnUpdateContent()
    {
        base.OnUpdateContent();

        foreach (var item in Value)
        {
            Nodes.Add(Constructor(item));
        }
    }

    protected override void OnUpdateAppearance()
    {
        base.OnUpdateAppearance();
        Text = $"{Text} [{Value.Count}]";
    }
}
