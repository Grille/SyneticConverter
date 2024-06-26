﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DarkUI.Controls;
using DarkUI.Docking;

using SyneticLib.Locations;

using SyneticTool.Nodes;

namespace SyneticLib.WinForms.Controls;

public partial class ProjectExplorerTool : DarkToolWindow
{
    public ProjectExplorerTool()
    {
        InitializeComponent();

        if (AppSettings.Games.Count == 0)
        {
            AppSettings.Setup();
        }

        darkTreeView1.MouseScrollHorizontalIfVerticalNotAvailable = false;

        foreach (var game in AppSettings.Games)
        {
            var node = new GameDirectoryNode(game);
            darkTreeView1.Nodes.Add(node);



            darkTreeView1.MouseWheel += DarkTreeView1_MouseWheel;
        }
    }

    private void DarkTreeView1_MouseWheel(object? sender, MouseEventArgs e)
    {

        //e.Delta
        //throw new NotImplementedException();
    }
}
