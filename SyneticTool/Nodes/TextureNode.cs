using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SyneticLib;

namespace SyneticTool.Nodes;

internal class TextureNode : DataTreeNode
{
    public TextureNode(Texture texture)
    {
        Text = texture.FileName;

        SelectedImageIndex = ImageIndex = IconList.Texture;
    }
}
