using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SyneticLib;

namespace SyneticTool.Nodes;

internal class ModelDirectoryNode : RessourceListNode<Model>
{
    public ModelDirectoryNode(RessourceList<Model> meshes, string name = "Objects") : base(meshes, (a) => new ModelNode(a))
    {
        Text = name;

        //var texnode = new TextureDirectoryNode(meshes.TextureFolder);
        //Nodes.Add(texnode);
    }
}
