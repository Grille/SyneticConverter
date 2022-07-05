using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SyneticConverter;

namespace SyneticTool;

public partial class MainForm : Form
{
    public Games Games;
    public MainForm()
    {
        InitializeComponent();
        Games = new Games();
    }

    private void addGameToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var dialog = new AddGameDialog();
        dialog.ShowDialog(this);

        if (dialog.DialogResult == DialogResult.OK)
        {
            Games.AddGame(dialog.GameName, dialog.GamePath);
            UpdateTree();
        }
    }

    private void convertToToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    public void UpdateTree()
    {
        dataTreeView.BeginUpdate();

        dataTreeView.Nodes.Clear();

        foreach (var game in Games.GameFolders)
        {
            var node = new GameFolderNode(game.Value);
            dataTreeView.Nodes.Add(node);
        }

        dataTreeView.EndUpdate();
    }

    private void MainForm_Shown(object sender, EventArgs e)
    {
        Games.AddGame("MBWR", "C:/Program Files (x86)/World Racing", GameVersion.MBWR);

        UpdateTree();
    }
}
