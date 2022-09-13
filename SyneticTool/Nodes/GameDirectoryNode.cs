using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SyneticLib;
using SyneticLib.IO.Extern;

namespace SyneticTool.Nodes;

public class GameDirectoryNode : DataTreeNode
{
    public new GameDirectory DataValue { get => (GameDirectory)base.DataValue; set => DataValue = value; }

    DataListTreeNode<Scenario> ScenariosNode;
    DataListTreeNode<Car> CarsNode;
    DataListTreeNode<Sound> SoundsNode;

    public GameDirectoryNode(GameDirectory game) : base(game)
    {
        ScenariosNode = new(game.Scenarios, (a) => new ScenarioNode(a));
        CarsNode = new(game.Cars, (a) => new CarNode(a));
        SoundsNode = new(game.Sounds, (a) => new SoundNode(a));

        Nodes.Add(ScenariosNode);
        Nodes.Add(CarsNode);
        Nodes.Add(SoundsNode);

        var menu = new ContextMenuStrip();

        var entry0 = new ToolStripMenuItem("Edit");
        var entry1 = new ToolStripMenuItem("Remove");

        entry0.Click += (object sender, EventArgs e) =>
        {
            MainForm.ShowAddOrEditGameDialog(DataValue);
        };
        entry1.Click += (object sender, EventArgs e) =>
        {
            MainForm.Games.Remove(DataValue);
            MainForm.Config.Save();
            MainForm.RefreshGamesTree();
        };

        menu.Items.Add(entry0);
        menu.Items.Add(entry1);
        ContextMenuStrip = menu;
    }

    protected override void OnUpdateAppearance()
    {
        base.OnUpdateAppearance();

        Name = $"{DataValue.Version} ({DataValue.SourcePath})";
        Text = Name;

        SelectedImageIndex = ImageIndex = DataValue.Version switch
        {
            GameVersion.NICE => IconList.NICE,
            GameVersion.NICE2 => IconList.NICE2,
            GameVersion.MBTR => IconList.MBTR,
            GameVersion.MBWR => IconList.MBWR,
            GameVersion.WR2 => IconList.WR2,
            GameVersion.C11 => IconList.C11,
            GameVersion.CT1 => IconList.CT1,
            GameVersion.CT2 => IconList.CT2,
            GameVersion.CT3 => IconList.CT3,
            GameVersion.CT4 => IconList.CT4,
            GameVersion.CT5 => IconList.CT5,
            GameVersion.FVR => IconList.FVR,
            _ => IconList.Default,
        };
    }


}
