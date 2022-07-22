using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SyneticLib;

namespace SyneticTool.Nodes;

public class CarNode : DataTreeNode
{
    public CarNode(Car car)
    {
        Text = car.Name;

        SelectedImageIndex = ImageIndex = IconList.Car;
    }
}
