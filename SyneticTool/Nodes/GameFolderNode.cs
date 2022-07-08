using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SyneticLib;

namespace SyneticTool;

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
    }


}
