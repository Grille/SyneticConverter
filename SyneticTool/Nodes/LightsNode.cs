using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SyneticLib;
using SyneticLib.IO.Extern;

namespace SyneticTool.Nodes;

public class LightsNode : DataListTreeNode<Light>
{
    public LightsNode(RessourceList<Light> lights) : base(lights, (a) => new DataTreeNode(a))
    {
        Text = "Lights";

        SelectedImageIndex = ImageIndex = IconList.Misc;

        var menu = new ContextMenuStrip();

        var entry = new ToolStripMenuItem("Export as BeamNG.JSON");
        entry.Click += (object sender, EventArgs e) =>
        {
            using var dialog = new SaveFileDialog();
            dialog.FileName = "items.level.json";
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                var export = new LightExportBeamNgJSON(lights);
                export.Save(dialog.FileName);
            }
        };

        menu.Items.Add(entry);
        ContextMenuStrip = menu;

    }


}
