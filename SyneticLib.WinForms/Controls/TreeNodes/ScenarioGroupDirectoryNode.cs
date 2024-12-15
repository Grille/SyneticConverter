using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DarkUI.Controls;

using SyneticLib.Locations;
using SyneticLib.WinForms.Resources;

namespace SyneticLib.WinForms.Controls.TreeNodes;

public class ScenarioGroupDirectoryNode : ArrayNode<DirectoryLocation>
{
    public ScenarioGroupDirectoryNode(DirectoryLocation location) : base(location.Name, (a) => new ScenarioDirectoryNode(a))
    {
        Icon = EmbeddedImageList.World.Bitmap16;

        var dir = DirectoryLocation.FromStringArray( Directory.GetDirectories(location.DirectoryPath));

        SetItems(dir);
    }
}
