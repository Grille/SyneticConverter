using SyneticLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticTool.Nodes;

public class ModelNode : DataTreeNode
{
    public Model Model => (Model)base.Ressource;

    public ModelNode(Model data) : base(data)
    {
        SelectedImageIndex = ImageIndex = IconList.Mesh;

        var menu = new ContextMenuStrip();

        var entry = new ToolStripMenuItem("Export as Obj");
        entry.Click += (object sender, EventArgs e) =>
        {
            using var dialog = new SaveFileDialog();
            //dialog.FileName = $"{DataValue.FileName}.obj";
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                //DataValue.ExportAsObj(dialog.FileName);
            }
        };

        menu.Items.Add(entry);
        ContextMenuStrip = menu;
    }

    public override void OnSelect(TreeViewCancelEventArgs e)
    {
        //if (DataValue.NeedLoad)
        //    DataValue.Load();
        //MainForm.Display.ShowMesh(DataValue);
    }
}
