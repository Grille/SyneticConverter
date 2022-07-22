using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SyneticLib;

namespace SyneticTool.Nodes;

internal class MeshFolderNode : DataTreeNode
{
    public MeshFolderNode(MeshFolder meshes, string name = "Objects")
    {
        Text = name;

        var texnode = new TextureFolderNode(meshes.TextureFolder);
        Nodes.Add(texnode);
    }
}
