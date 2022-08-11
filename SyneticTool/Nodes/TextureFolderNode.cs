using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SyneticLib;

namespace SyneticTool.Nodes;

internal class TextureFolderNode : DataTreeNode
{
    TextureDirectory textures;



    public TextureFolderNode(TextureDirectory textures, string name = "Textures")
    {
        this.textures = textures;
        Text = name;

        Update();
    }

    public void Update()
    {
        Nodes.Clear();

        foreach (var texture in textures)
        {
            Nodes.Add(new TextureNode(texture));
        }
    }
}
