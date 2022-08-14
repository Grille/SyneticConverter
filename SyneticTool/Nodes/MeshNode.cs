﻿using SyneticLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticTool.Nodes;

internal class MeshNode : DataTreeNode
{
    public MeshNode(Mesh data) : base(data)
    {
        SelectedImageIndex = ImageIndex = IconList.Mesh;
    }
}