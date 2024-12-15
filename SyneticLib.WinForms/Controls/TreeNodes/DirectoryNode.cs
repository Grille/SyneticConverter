using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DarkUI.Controls;

using SyneticLib.WinForms.Resources;

namespace SyneticLib.WinForms.Controls.TreeNodes;

public class DirectoryNode : DarkTreeNode
{
    public DarkTreeNode Owner { get; }
    public string DirectoryPath { get; }

    public string DirectoryName => Path.GetFileName(DirectoryPath);

    public string Name { get; set; }

    bool _init = false;

    public DirectoryNode(string dirPath, DarkTreeNode owner)
    {
        Owner = owner;

        DirectoryPath = dirPath;

        Name = DirectoryName;
        Text = Name;

        Icon = EmbeddedImageList.Folder.Bitmap16;

        Owner.NodeExpanded += Owner_NodeExpanded;
    }

    private void Owner_NodeExpanded(object? sender, EventArgs e)
    {
        Init();
    }

    public void Init()
    {
        if (_init)
            return;
        _init = true;
        Update();
    }

    public void Update()
    {
        Nodes.Clear();

        var list = new List<DarkTreeNode>();

        var directories = Directory.GetDirectories(DirectoryPath);
        foreach (var dir in directories)
        {
            var node = new DirectoryNode(dir, this);
            list.Add(node);
        }

        var files = Directory.GetFiles(DirectoryPath);
        foreach (var file in files)
        {
            var node = new FileNode(file);
            list.Add(node);
        }

        Text = $"{Name} [{list.Count}]";

        Nodes.AddRange(list);

        ParentTree.Invalidate();
    }
}
