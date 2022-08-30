using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SyneticLib;

namespace SyneticTool.Nodes;

public class GameFolderNode : DataTreeNode
{
    public new GameFolder DataValue { get => (GameFolder)base.DataValue; set => DataValue = value; }

    DataListTreeNode<Scenario> ScenariosNode;
    DataListTreeNode<Car> CarsNode;
    DataListTreeNode<Sound> SoundsNode;

    public GameFolderNode(GameFolder game) : base(game)
    {
        ScenariosNode = new(game.Scenarios, (a) => new ScenarioNode(a));
        CarsNode = new(game.Cars, (a) => new CarNode(a));
        SoundsNode = new(game.Sounds, (a) => new SoundNode(a));

        Nodes.Add(ScenariosNode);
        Nodes.Add(CarsNode);
        Nodes.Add(SoundsNode);
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
            GameVersion.MBWR => IconList.MBWR,
            GameVersion.WR2 => IconList.WR2,
            GameVersion.C11 => IconList.C11,
            GameVersion.CTP => IconList.CT1,
            GameVersion.CT2 => IconList.CT2,
            GameVersion.CT3 => IconList.CT3,
            GameVersion.CT4 => IconList.CT4,
            GameVersion.CT5 => IconList.CT5,
            GameVersion.FVR => IconList.FVR,
            _ => IconList.Default,
        };
    }


}
