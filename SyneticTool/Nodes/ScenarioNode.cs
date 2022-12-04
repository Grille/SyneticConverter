using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

using SyneticLib;

namespace SyneticTool.Nodes;

internal class ScenarioNode : DataTreeNode
{
    public new ScenarioVGroup DataValue { get => (ScenarioVGroup)base.DataValue; set => base.DataValue = value; }

    public ScenarioNode(ScenarioVGroup scenario) : base(scenario)
    {
        SelectedImageIndex = ImageIndex = IconList.World;
    }

    protected override void OnUpdateContent()
    {
        base.OnUpdateContent();

        Text = DataValue.FileName;

        var variants = DataValue.Variants;
        for (int i = 0; i < variants.Count; i++)
        {
            Nodes.Add(new ScenarioVariantNode(variants[i]));
        }
    }

    public ScenarioVariantNode V1
    {
        get => Nodes.Count > 0 ? (ScenarioVariantNode)Nodes[0] : null;
    }

    public override void OnSelect(TreeViewCancelEventArgs e)
    {
        if (DataValue.NeedLoad)
            DataValue.Load();
        MainForm.Display.ShowScenario(DataValue);
    }
}
