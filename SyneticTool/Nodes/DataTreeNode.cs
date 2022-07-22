using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticTool.Nodes;

public class DataTreeNode : TreeNode
{
    public DataTreeNode()
    {

    }

    public DataTreeNode(string name)
    {
        Name = name;
        Text = name;
    }

    public int Image
    {
        get => ImageIndex;
        set
        {
            SelectedImageIndex = ImageIndex = value;
        }
    }
}
