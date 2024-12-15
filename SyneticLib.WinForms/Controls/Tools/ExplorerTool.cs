using System;
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
using DarkUI.Forms;

using SyneticLib.Locations;
using SyneticLib.WinForms.Forms;

using SyneticTool.Nodes;

namespace SyneticLib.WinForms.Controls;

public partial class ExplorerTool : DarkToolWindow
{
    public EditorForm? Owner;

    public ExplorerTool()
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

    private void toolStripButton3_Click(object sender, EventArgs e)
    {
        AppSettings.SearchGamesDialog(this);
    }

    private void toolStripButton2_Click(object sender, EventArgs e)
    {
        var nodes = darkTreeView1.SelectedNodes.ToArray();

        foreach (var node in nodes)
        {
            if (node is not GameDirectoryNode)
            {
                DarkMessageBox.ShowInformation($"{node.Text} is not a game location.", "Invalid Selection", DarkDialogButton.Ok);
                return;
            }
            darkTreeView1.Nodes.Remove(node);
        }
    }
}
