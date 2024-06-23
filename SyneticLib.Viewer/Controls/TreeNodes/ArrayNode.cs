using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DarkUI.Controls;

using SyneticLib.Locations;

namespace SyneticLib.WinForms.Controls.TreeNodes;

public class ArrayNode<T> : DarkTreeNode
{
    public string Name { get; }
    public T[] Items { get; private set; }

    readonly Func<T, DarkTreeNode> _factory;

    public ArrayNode(string name, Func<T, DarkTreeNode> factory)
    {
        _factory = factory;
        Name = name;
        SetItems(Array.Empty<T>());
    }

    [MemberNotNull(nameof(Items))]
    public void SetItems(T[] items)
    {
        Nodes.Clear();
        Items = items;

        Text = $"{Name} [{Items.Length}]";

        var nodes = new DarkTreeNode[Items.Length];
        for (int i = 0; i < Items.Length; i++)
        {
            nodes[i] = _factory(items[i]);
        }

        Nodes.AddRange(nodes);
    }
}
