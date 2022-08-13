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
    public new ScenarioVariant DataValue { get => (ScenarioVariant)base.DataValue; set => base.DataValue = value; }

    public DataTreeNode TerrainNode;
    public TextureDirectoryNode TerrainTexturesNode;
    public MeshDirectoryNode ObjectsNode;
    public TextureDirectoryNode ObjectTexturesNode;
    public LightsNode LightsNode;

    public ScenarioVariantNode(ScenarioVariant variant) : base(variant)
    {
        SelectedImageIndex = ImageIndex = IconList.Terrain;
        Text = variant.FileName;

        TerrainNode = new(variant.Terrain);
        TerrainTexturesNode = new(variant.TerrainTextures, "Terrain-Textures");
        ObjectsNode = new(variant.Objects);
        ObjectTexturesNode = new(variant.ObjectTextures, "Object-Textures");
        LightsNode = new(variant.Lights);

        Nodes.Add(TerrainTexturesNode);
        Nodes.Add(ObjectTexturesNode);
        Nodes.Add(ObjectsNode);
        Nodes.Add(LightsNode);
        Nodes.Add(TerrainNode);

    }
}
