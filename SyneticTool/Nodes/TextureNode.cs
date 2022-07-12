using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SyneticLib;

namespace SyneticTool.Nodes;

internal class TextureNode : TreeNode
{
    public TextureNode(Texture texture)
    {
        Text = texture.Name;

        SelectedImageIndex = ImageIndex = IconList.Texture;
    }
}
