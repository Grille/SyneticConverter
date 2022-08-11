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
    public ScenarioVariant Value;
    public ScenarioVariantNode(string folder, ScenarioVariant variant)
    {
        Text = folder;
        Value = variant;

        var texnode = new TextureFolderNode(variant.WorldTextures, "Textures");
        var objnode = new MeshFolderNode(variant.Objects);
        var lightnode = new LightsNode(variant.Lights);

        Nodes.Add(texnode);
        Nodes.Add(objnode);
        Nodes.Add(lightnode);

        SelectedImageIndex = ImageIndex = IconList.Terrain;
    }

    public void UpdateColor()
    {
        ForeColor = Value.DataState switch
        {
            DataState.Loaded => NodeColors.Changed,
            DataState.Error => NodeColors.Failed,
            _ => NodeColors.Default,
        };
    }
}
