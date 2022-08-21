using SyneticLib;
using SyneticLib.IO.Extern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticTool.Nodes;

public class MeshNode : DataTreeNode
{
    public new Mesh DataValue { get => (Mesh)base.DataValue; set => base.DataValue = value; }

    public MeshNode(Mesh data) : base(data)
    {
        SelectedImageIndex = ImageIndex = IconList.Mesh;

        var menu = new ContextMenuStrip();

        var entry = new ToolStripMenuItem("Export as Obj");
        entry.Click += (object sender, EventArgs e) =>
        {
            using var dialog = new SaveFileDialog();
            dialog.FileName = $"{DataValue.FileName}.obj";
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                DataValue.ExportAsObj(dialog.FileName);
            }
        };

        menu.Items.Add(entry);
        ContextMenuStrip = menu;
    }
}
