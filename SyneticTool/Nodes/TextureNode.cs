using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SyneticLib;
using SyneticTool.Nodes.System;

namespace SyneticTool.Nodes;

internal class TextureNode : RessourceNode
{
    public new Texture DataValue { get => (Texture)base.Ressource; set => DataValue = value; }

    public TextureNode(Texture texture) : base(texture)
    {
        Text = texture.Name;

        SelectedImageIndex = ImageIndex = IconList.Texture;
    }

    public override void OnSelect(TreeViewCancelEventArgs e)
    {
        //MainForm.Display.ShowTexture(DataValue);
    }
}
