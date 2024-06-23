using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DarkUI.Controls;

namespace SyneticLib.WinForms.Controls.TreeNodes;

public abstract class SmartDarkTreeNode<T> : DarkTreeNode
{
    public T Object { get; }

    bool build = false;

    public SmartDarkTreeNode(T obj)
    {
        Object = obj;

        NodeExpanded += SmartDarkTreeNode_NodeExpanded;
    }

    private void SmartDarkTreeNode_NodeExpanded(object? sender, EventArgs e)
    {
        if (build == true)
            return;

        build = true;

        UpdateContent();
    }

    public void UpdateContent()
    {
        OnUpdateContent();
    }

    protected abstract void OnUpdateContent();
}
