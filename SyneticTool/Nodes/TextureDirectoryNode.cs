﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SyneticLib;

namespace SyneticTool.Nodes;

internal class TextureDirectoryNode : DataListTreeNode<Texture>
{

    public TextureDirectoryNode(TextureDirectory textures, string name = "Textures") : base(textures, (a) => new TextureNode(a))
    {
        Text = name;
    }

    public void Update()
    {
        Nodes.Clear();

        foreach (var texture in DataValue)
        {
            Nodes.Add(new TextureNode(texture));
        }
    }
}