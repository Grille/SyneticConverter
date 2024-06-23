using SyneticLib;
using SyneticLib.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticTool.Nodes;

public class LocationTreeNode : BaseNode
{
    public DirectoryLocation Location => (DirectoryLocation)Value;

    public LocationTreeNode(DirectoryLocation location) : base(location)
    {
    }
}
