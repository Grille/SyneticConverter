using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

using SyneticLib;
using SyneticTool.Nodes.System;

namespace SyneticTool.Nodes;

internal class ScenarioNode : LazyRessourceNode<ScenarioGroup>
{
    public ScenarioNode(Lazy<ScenarioGroup> scenario) : base(scenario)
    {
        SelectedImageIndex = ImageIndex = IconList.World;
    }

    protected override void OnUpdateContent()
    {
        base.OnUpdateContent();

        var value = Value.Value;

        Text = value.Name;

        
        var variants = value.Variants;
        for (int i = 0; i < variants.Length; i++)
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
        MainForm.Display.ShowScenario(Value.Value);
    }
}
