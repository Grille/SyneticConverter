using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

using SyneticLib;

namespace SyneticTool.Nodes;

internal class ScenarioNode : TreeNode
{
    public Scenario Value;
    public ScenarioNode(string folder, Scenario scenario)
    {
        Text = folder;
        Value = scenario;

        var variants = scenario.Variants;
        for (int i = 0; i < variants.Length; i++)
        {
            Nodes.Add(new ScenarioVariantNode($"V{i + 1}", variants[i]));
        }

        SelectedImageIndex = ImageIndex = IconList.World;
    }

    public ScenarioVariantNode V1
    {
        get => Nodes.Count > 0 ? (ScenarioVariantNode)Nodes[0] : null;
    }

    public void UpdateColor()
    {
        ForeColor = Value.State switch
        {
            InitState.Initialized => Color.Green,
            InitState.Failed => Color.Red,
            _ => Color.Black,
        };
    }
}
