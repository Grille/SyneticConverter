using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SyneticLib.Locations;
using SyneticLib;
using DarkUI.Controls;
using SyneticLib.WinForms.Resources;
using SyneticLib.WinForms.Controls.TreeNodes;
using static SyneticLib.IO.Serializers;

namespace SyneticTool.Nodes;

public class GameDirectoryNode : SmartDarkTreeNode<GameDirectory>
{
    public GameDirectory GameDirectory => Object;

    public ArrayNode<DirectoryLocation> ScenariosNode { get; }

    public GameDirectoryNode(GameDirectory game) : base(game)
    {
        UpdateAppearance();

        ScenariosNode = new($"Scenarios", (a) => new ScenarioGroupDirectoryNode(a));
        Nodes.Add(ScenariosNode);
    }

    protected override void OnUpdateContent()
    {
        var scenarios = GameDirectory.GetScenarios();
        ScenariosNode.SetItems(scenarios);
    }

    protected void UpdateAppearance()
    {

        Text = $"{GameDirectory.Version} ({GameDirectory.DirectoryPath})";

        IconArea = new System.Drawing.Rectangle(0, 0, 100, 100);

        TextArea = new System.Drawing.Rectangle(0,0,10,10);


        var image = GameDirectory.Version switch
        {
            GameVersion.NICE1 => EmbeddedImageList.NICE,
            GameVersion.NICE2 => EmbeddedImageList.NICE2,
            GameVersion.MBTR => EmbeddedImageList.MBTR,
            GameVersion.WR1 => EmbeddedImageList.MBWR,
            GameVersion.WR2 => EmbeddedImageList.WR2,
            GameVersion.C11 => EmbeddedImageList.C11,
            GameVersion.CT1 => EmbeddedImageList.CT1,
            GameVersion.CT2 => EmbeddedImageList.CT2,
            GameVersion.CT3 => EmbeddedImageList.CT3,
            GameVersion.CT4 => EmbeddedImageList.CT4,
            GameVersion.CT5 => EmbeddedImageList.CT5,
            GameVersion.FVR => EmbeddedImageList.FVR,
            _ => EmbeddedImageList.SyneticLib,
        };

        Icon = image.Bitmap16;
    }
}
