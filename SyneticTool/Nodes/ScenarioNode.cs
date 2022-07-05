using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SyneticConverter;

namespace SyneticTool;

internal class ScenarioNode : TreeNode
{
    public ScenarioNode(string folder, Scenario scenario)
    {
        this.Text = folder;

        var menuItemInspect = new ToolStripMenuItem("Inspect");
        menuItemInspect.Click += (object sender, EventArgs e) =>
        {
            new ScenarioInspector(scenario).Show();
        };

        var menuItemExport = new ToolStripMenuItem("Export");
        menuItemExport.Click += (object sender, EventArgs e) =>
        {

        };
        ContextMenuStrip = new ContextMenuStrip();
        ContextMenuStrip.Items.Add(menuItemInspect);
        ContextMenuStrip.Items.Add(menuItemExport);
    }
}
