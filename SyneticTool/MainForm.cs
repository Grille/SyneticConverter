using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.WinForms;
using System.Numerics;
using SyneticLib.Graphics;
using SyneticTool.Nodes;
using Graphics = SyneticLib.Graphics;
using SyneticLib.Locations;

namespace SyneticTool;

public partial class MainForm : Form
{
    public GameDirectoryList Games;
    GLControl glControl;
    public Config Config;
    public Display Display;
    Task LoadingTask;
    Task QueuedLoadingTask;

    public MainForm()
    {
        InitializeComponent();
        DoubleBuffered = true;
        dataTreeView.ImageList = IconList.Images;

        glControl = new GLControl(new()
        {
            API = OpenTK.Windowing.Common.ContextAPI.OpenGL,
            APIVersion = new Version(4, 5, 0, 0),
            Flags = OpenTK.Windowing.Common.ContextFlags.Debug,


        });
        glControl.BackColor = Color.Black;
        glControl.Dock = DockStyle.Fill;
        glPanel.MouseWheel += GlPanel_MouseWheel;
        glControl.MouseMove += GlPanel_MouseMove;
        glPanel.Controls.Add(glControl);


        Config = new("config.dat");
        Config.TryLoad();

        Games = Config.Games;
    }

    private void GlPanel_MouseMove(object sender, MouseEventArgs e)
    {
        Display.Camera.MouseMove(e.X, e.Y, e.Button == MouseButtons.Left);
    }

    private void GlPanel_MouseWheel(object sender, MouseEventArgs e)
    {
        Display.Camera.Scroll(e.Delta);
    }

    private void GlControl_Paint(object sender, PaintEventArgs e) => Display.Render();


    private void addGameToolStripMenuItem_Click(object sender, EventArgs e)
    {
        ShowAddOrEditGameDialog();
    }

    private void convertToToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }



    private void MainForm_Shown(object sender, EventArgs e)
    {
        Display = new(glControl);
        renderTimer.Start();

        if (Games.Count == 0)
        {
            var games = Games.FindNewGames();
            if (games.Count > 0)
            {
                ShowApplyGamesDialog(games);
            }
            else
            {
                ShowAddOrEditGameDialog();
            }
        }

        RefreshGamesTree();
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {

    }

    private void renderTimer_Tick(object sender, EventArgs e) => Display.Render();


    private void dataTreeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
        /*
        //dataTreeView.BeginUpdate();

        switch (e.Node)
        {
            case CarNode:
            {
                var mnode = (CarNode)e.Node;

                if (mnode.DataValue.NeedLoad)
                    mnode.DataValue.Load();
                mnode.UpdateAppearance();

                Display.ShowMesh((Model)mnode.MeshNode.DataValue);
            }
            break;
            case ModelNode:
            {
                var mnode = (ModelNode)e.Node;

                if (mnode.DataValue.NeedLoad)
                    mnode.DataValue.Load();
                mnode.UpdateAppearance();

                Display.ShowMesh((Model)mnode.DataValue);
            }
            break;
            case TextureNode:
            {
                var tnode = (TextureNode)e.Node;

                if (tnode.DataValue.NeedLoad)
                    tnode.DataValue.Load();
                tnode.UpdateAppearance();

                Display.ShowTexture((Texture)tnode.DataValue);
            }
            break;
            case ScenarioNode:
            {
                var snode = (ScenarioNode)e.Node;
                var vnode = snode.V1;

                if (vnode.DataValue.NeedLoad)
                {
                    var dialog = new ProgressForm(vnode.DataValue);
                    dialog.Show();
                    vnode.DataValue.Load();
                    dialog.Close();
                }
                vnode.UpdateAppearance();
                Display.ShowScenario(vnode.DataValue);
            }
            break;
            case ScenarioVariantNode:
            {
                var snode = (ScenarioNode)e.Node.Parent;
                var vnode = (ScenarioVariantNode)e.Node;

                if (vnode.DataValue.NeedLoad)
                    vnode.DataValue.Load();
                vnode.UpdateAppearance();
                Display.ShowScenario(vnode.DataValue);
            }
            break;
        }

        //dataTreeView.EndUpdate();
        */
    }


    private void dataTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
    {
        dataTreeView.BeginUpdate();

        if (e.Node is MyTreeNode)
        {
            var node = (MyTreeNode)e.Node;
            node.OnExpand(e);
            foreach (var cnode in node.Nodes)
            {
                if (cnode is MyTreeNode)
                {
                    ((MyTreeNode)cnode).OnShown();
                }
            }
        }

        dataTreeView.EndUpdate();
    }

    private void detectGamesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var games = Games.FindNewGames();
        ShowApplyGamesDialog(games);
    }

    public void RefreshGamesTree()
    {
        List<GameDirectoryNode> listToAdd = new();
        List<GameDirectoryNode> listToRemove = new();

        foreach (var node in dataTreeView.Nodes)
        {
            var gnode = (GameDirectoryNode)node;
            if (!Games.Contains(gnode.GameDirectory))
            {
                listToRemove.Add(gnode);
            }
        }

        foreach (var game in Games)
        {
            bool found = false;
            foreach (var node in dataTreeView.Nodes)
            {
                var gnode = (GameDirectoryNode)node;
                if (gnode.GameDirectory == game)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                var node = new GameDirectoryNode(game);
                node.OnShown();
                listToAdd.Add(node);
            }

        }

        if (listToAdd.Count == 0 && listToRemove.Count == 0)
            return;


        dataTreeView.BeginUpdate();

        foreach (var node in listToAdd)
            dataTreeView.Nodes.Add(node);

        foreach (var node in listToRemove)
            dataTreeView.Nodes.Remove(node);

        dataTreeView.EndUpdate();
    }

    public void ShowAddOrEditGameDialog(GameDirectory target = null)
    {
        var dialog = target == null ? new AddGameDialog(Games) : new AddGameDialog(Games, target);
        dialog.ShowDialog(this);

        if (dialog.DialogResult == DialogResult.OK)
        {
            dialog.ApplyToGame();
            var game = dialog.SelectedGame;

            if (game == null)
                return;

            if (Games.Contains(game))
            {
                foreach (var node in dataTreeView.Nodes)
                {
                    var gfnode = (GameDirectoryNode)node;
                    if (gfnode.GameDirectory == game)
                    {
                        gfnode.OnShown();
                    }
                }
            }
            else
            {
                Games.Add(game);

                RefreshGamesTree();
            }

            Config.Save();
        }
    }

    public void ShowApplyGamesDialog(List<GameDirectory> games)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Found {games.Count} new game locations.");
        if (games.Count > 0)
        {
            sb.AppendLine();
            foreach (var game in games)
            {
                sb.AppendLine($"{game.Version} {game.Path}");
            }
        }
        var result = MessageBox.Show(this, sb.ToString(), "Find Games", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        if (result == DialogResult.OK)
        {
            foreach (var game in games)
            {
                Games.Add(game);
            }
            Config.Save();

            RefreshGamesTree();
        }
    }

    private void dataTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
    {
        if (e.Node is MyTreeNode)
        {
            var node = (MyTreeNode)e.Node;
            node.OnSelect(e);


            /*
            dataTreeView.BeginUpdate();

            var node = (DataTreeNode)e.Node;
            foreach (var n in node.Nodes)
            {
                if (!(n is DataTreeNode))
                    continue;

                var cnode = (DataTreeNode)n;
                cnode.SeekAndUpdateContent();
            }

            dataTreeView.EndUpdate();
            */
        }
    }
}
