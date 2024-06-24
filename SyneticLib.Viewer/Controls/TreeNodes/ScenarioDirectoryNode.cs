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

public class ScenarioDirectoryNode : SmartDarkTreeNode<DirectoryLocation>
{
    public DirectoryNode Files;
    //public ArrayNode<DirectoryLocation> ObjectTextures;
    //public ArrayNode<DirectoryLocation> Objects;

    public ScenarioDirectoryNode(DirectoryLocation obj) : base(obj)
    {
        Text = obj.Name;
        Icon = EmbeddedImageList.World.Bitmap16;

        Files = new DirectoryNode(Object.DirectoryPath, this)
        {
            Name = "Files:"
        };
        Nodes.Add(Files);
    }

    protected override void OnUpdateContent()
    {
        var path = Object.ChildPath("Textures");

        var files = Directory.GetFiles(path);

        //GroundTextures.SetItems(files);
    }
}
