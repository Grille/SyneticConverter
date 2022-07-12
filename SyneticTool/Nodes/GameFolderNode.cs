using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SyneticLib;

namespace SyneticTool.Nodes;

public class GameFolderNode : TreeNode
{
    public GameFolderNode(GameFolder game)
    {
        Name = $"{game.Version} ({game.RootDir})";
        Text = Name;

        var scenariosNode = new TreeNode("Scenarios");
        var carsNode = new TreeNode("Cars");

        game.Refresh();

        foreach (var scenario in game.Scenarios)
        {
            scenariosNode.Nodes.Add(new ScenarioNode(scenario.Key, scenario.Value));
        }

        foreach (var car in game.Cars)
        {
            carsNode.Nodes.Add(new CarNode(car.Key, car.Value));
        }

        Nodes.Add(scenariosNode);
        Nodes.Add(carsNode);

        SelectedImageIndex = ImageIndex = game.Version switch
        {
            GameVersion.MBWR => IconList.MBWR,
            GameVersion.WR2 => IconList.WR2,
            GameVersion.C11 => IconList.C11,
            GameVersion.CT1AP => IconList.CT1,
            GameVersion.CT2BW => IconList.CT2,
            GameVersion.CT3HN => IconList.CT3,
            GameVersion.CT4TS => IconList.CT4,
            GameVersion.CT5U => IconList.CT5,
            GameVersion.FVR => IconList.FVR,
            _ => IconList.Default,
        };
    }


}
