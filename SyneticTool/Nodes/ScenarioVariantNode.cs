using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

using SyneticLib;

namespace SyneticTool.Nodes;

internal class ScenarioVariantNode : DataTreeNode
{
    public new Scenario DataValue => (Scenario)base.Ressource;

    public DataTreeNode TerrainNode;
    public TextureDirectoryNode TerrainTexturesNode;
    public ModelDirectoryNode ObjectsNode;
    public TextureDirectoryNode ObjectTexturesNode;
    public LightsNode LightsNode;
    public PropListNode PropsNode;

    public ScenarioVariantNode(Scenario variant) : base(variant)
    {
        Image = IconList.Terrain;
        Text = variant.Name;

        TerrainNode = new(variant.Terrain);
        TerrainNode.Image = IconList.Terrain;

        //TerrainTexturesNode = new(variant.TerrainTextures, "Terrain-Textures");
        //ObjectsNode = new(variant.Models);
        //ObjectTexturesNode = new(variant.ModelTextures, "Object-Textures");
        LightsNode = new(variant.Lights);
        PropsNode = new(variant.PropClasses);

        //Nodes.Add(TerrainTexturesNode);
        //Nodes.Add(ObjectTexturesNode);
        //Nodes.Add(ObjectsNode);
        Nodes.Add(TerrainNode);
        Nodes.Add(LightsNode);
        Nodes.Add(PropsNode);
    }

    public override void OnSelect(TreeViewCancelEventArgs e)
    {
        MainForm.Display.ShowScenario(DataValue);
    }
}
