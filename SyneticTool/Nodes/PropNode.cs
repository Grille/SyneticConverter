﻿using SyneticLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticTool.Nodes;

public class PropNode : DataTreeNode
{
    public new PropClass DataValue => (PropClass)base.Ressource;

    public PropNode(PropClass data) : base(data)
    {
        Nodes.Add($"{data.Name}");
    }

    protected override void OnUpdateAppearance()
    {
        base.OnUpdateAppearance();
        Text = $"{DataValue.Name}";
    }
}
