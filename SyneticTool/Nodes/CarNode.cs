using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SyneticLib;

namespace SyneticTool;

public class CarNode : TreeNode
{
    public CarNode(string name, Car car)
    {
        Text = name;
    }
}
