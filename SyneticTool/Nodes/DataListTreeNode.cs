using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib;

namespace SyneticTool.Nodes;

public class DataListTreeNode<T> : DataTreeNode where T : Ressource
{
    public new RessourceList<T> DataValue { get => (RessourceList<T>)base.DataValue; set => base.DataValue = value; }

    public Func<T, DataTreeNode> Constructor;

    public DataListTreeNode(RessourceList<T> list, Func<T, DataTreeNode> constructor) :base(list)
    {
        Constructor = constructor;
    }

    protected override void OnUpdateContent()
    {
        base.OnUpdateContent();

        foreach (var item in DataValue)
        {
            Nodes.Add(Constructor(item));
        }
    }

    protected override void OnUpdateAppearance()
    {
        base.OnUpdateAppearance();
        Text = $"{Text} [{DataValue.Count}]";
    }
}
