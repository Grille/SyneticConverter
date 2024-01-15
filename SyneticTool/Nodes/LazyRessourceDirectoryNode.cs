using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib;
using SyneticLib.Locations;

namespace SyneticTool.Nodes;

public class LazyRessourceDirectoryNode<T> : BaseNode where T : SyneticObject
{
    public new LazyRessourceDirectory<T> Value => (LazyRessourceDirectory<T>)base.Value;

    public Func<Lazy<T>, LazyRessourceNode<T>> Constructor;

    public LazyRessourceDirectoryNode(LazyRessourceDirectory<T> list, Func<Lazy<T>, LazyRessourceNode<T>> constructor) :base(list)
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
