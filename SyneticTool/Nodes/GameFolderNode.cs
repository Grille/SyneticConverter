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
    public GameFolderNode(GameFolder game)
    {
        Name = $"{game.Version} ({game.SourcePath})";
        Text = Name;

        var scenariosNode = new DataTreeNode("Scenarios");
        var carsNode = new DataTreeNode("Cars");
        var soundsNode = new DataTreeNode("Sounds");


        ForeColor = NodeColors.RessourceColor(game);
        scenariosNode.ForeColor = ForeColor;
        carsNode.ForeColor = ForeColor;
        soundsNode.ForeColor = ForeColor;

        foreach (var scenario in game.Scenarios)
        {
            scenariosNode.Nodes.Add(new ScenarioNode(scenario));
        }

        foreach (var car in game.Cars)
        {
            carsNode.Nodes.Add(new CarNode(car));
        }

        Nodes.Add(scenariosNode);
        Nodes.Add(carsNode);
        Nodes.Add(soundsNode);

        SelectedImageIndex = ImageIndex = game.Version switch
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
