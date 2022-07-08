using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

using SyneticLib;

namespace SyneticTool;

internal class ScenarioNodeVariant : TreeNode
{
    public ScenarioVariant Value;
    public ScenarioNodeVariant(string folder, ScenarioVariant variant)
    {
        Text = folder;
        Value = variant;

        var texnode = new TreeNode("Textures");
        var objnode = new TreeNode("Objects");
        var objtexnode = new TreeNode("Textures");

        Nodes.Add(texnode);
        Nodes.Add(objnode);
        objnode.Nodes.Add(objtexnode);
    }

    public void UpdateColor()
    {
        ForeColor = Value.State switch
        {
            ScenarioState.Initialized => Color.Green,
            ScenarioState.Failed => Color.Red,
            _ => Color.Black,
        };
    }
}
