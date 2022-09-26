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
    public new Texture DataValue { get => (Texture)base.DataValue; set => DataValue = value; }

    public TextureNode(Texture texture) : base(texture)
    {
        Text = texture.FileName;

        SelectedImageIndex = ImageIndex = IconList.Texture;
    }

    public override void OnSelect(TreeViewCancelEventArgs e)
    {
        if (DataValue.NeedLoad)
            DataValue.Load();
        MainForm.Display.ShowTexture(DataValue);
    }
}
