using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib;
using SyneticLib.Locations;

namespace SyneticTool.Nodes;

public class DirectoryListTreeNode<T> : MyTreeNode where T : Ressource
{
    public RessourceDirectory<T> Directory => (RessourceDirectory<T>)base.Object;

    public Func<T, DataTreeNode> Constructor;

    public DirectoryListTreeNode(RessourceDirectory<T> list, Func<T, DataTreeNode> constructor) :base(list)
    {
        Constructor = constructor;
    }

    protected override void OnUpdateContent()
    {
        base.OnUpdateContent();

        foreach (var item in Directory)
        {
            Nodes.Add(Constructor(item));
        }
    }

    protected override void OnUpdateAppearance()
    {
        base.OnUpdateAppearance();
        Text = $"{Text} [{Directory.Count}]";
    }
}
