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


using SyneticLib;
using SyneticLib.Graphics;
using SyneticTool.Nodes;

namespace SyneticTool;

public partial class MainForm : Form
{
    public GameDirectoryList Games;
    GLControl glControl;
    public Config Config;
    Scene scene;
    Task LoadingTask;
    Task QueuedLoadingTask;

    public MainForm()
    {
        InitializeComponent();

        var settings = new GLControlSettings();
        settings.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
        settings.APIVersion = new Version(4, 5, 0, 0);
        glControl = new GLControl(settings);
        glControl.BackColor = Color.Black;
        glControl.Dock = DockStyle.Fill;
        //glControl.Visible = false;
        glPanel.Controls.Add(glControl);
        glPanel.MouseWheel += GlPanel_MouseWheel;
        glControl.MouseMove += GlPanel_MouseMove;
        dataTreeView.ImageList = IconList.Images;

        Config = new("config.dat");
        Config.TryLoad();

        Games = Config.Games;

        errorPanel.Visible = false;
        errorPanel.BringToFront();

        DoubleBuffered = true;

        scene = new();
    }

    private void GlPanel_MouseMove(object sender, MouseEventArgs e)
    {
        scene.Camera.MouseMove(e.X, e.Y, e.Button == MouseButtons.Left);
    }

    private void GlPanel_MouseWheel(object sender, MouseEventArgs e)
    {
        scene.Camera.Scroll(e.Delta);
    }

    private void GlControl_Paint(object sender, PaintEventArgs e)
    {
        RenderFrame();
    }


    private void addGameToolStripMenuItem_Click(object sender, EventArgs e)
    {
        ShowAddOrEditGameDialog();
    }

    private void convertToToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }


    public GameDirectoryList FindNewGames()
    {
        var res = new GameDirectoryList();

        var locations = new string[]
        {
            "",
            "Games",
            "Programs",
            "Programme",
            "Program Files",
            "Program Files (x86)",
            "Program Files\\steamapps\\steamapps\\common",
            "Program Files (x86)\\Steam\\steamapps\\common",
        };

        var names = new string[]
        {
            "", "TDK", "Synetic"
        };

        var drives = DriveInfo.GetDrives();

        
        foreach (var drive in drives)
        {
            foreach (var location in locations)
            {
                var path0 = Path.Join(drive.Name, location);
                if (!Directory.Exists(path0))
                    continue;

                foreach (var name in names)
                {
                    var path = Path.Join(path0, name);
                    if (!Directory.Exists(path))
                        continue;

                    string[] directory;
                    try
                    {
                        directory = Directory.GetDirectories(path);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        continue;
                    }
                    foreach (var fpath in directory)
                    {
                        GameVersion version;
                        try
                        {
                            version = GameDirectory.FindDirectoryGameVersion(fpath);
                        }
                        catch (UnauthorizedAccessException){
                            continue;
                        }
                        if (version != GameVersion.Invalid)
                        {
                            if (!Games.PathExists(fpath))
                            {
                                res.Add(new(fpath, version));
                            }
                        }
                    }
                }
            }
        }
        

        return res;
    }


    private void MainForm_Shown(object sender, EventArgs e)
    {
        renderTimer.Start();

        if (Games.Count == 0)
        {
            var games = FindNewGames();
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

    private void renderTimer_Tick(object sender, EventArgs e)
    {
        RenderFrame();
    }

    public void RenderFrame()
    {
        errorPanel.Location = new Point(glControl.ClientSize.Width / 2 - errorPanel.Width / 2, glControl.ClientSize.Height / 2 - errorPanel.Height / 2);
        scene.Camera.ScreenSize = new OpenTK.Mathematics.Vector2(glControl.ClientSize.Width, glControl.ClientSize.Height);
        scene.Camera.CreatePerspective();

        try
        {
            glControl.MakeCurrent();
        }
        catch (OpenTK.Windowing.GraphicsLibraryFramework.GLFWException)
        {
            return; // Context failed: drop frame
        }

        scene.ClearScreen();
        scene.Render();

        glControl.SwapBuffers();
    }

    private void dataTreeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
        //dataTreeView.BeginUpdate();

        switch (e.Node)
        {
            case CarNode:
            {
                var mnode = (CarNode)e.Node;

                if (mnode.DataValue.NeedLoad)
                    mnode.DataValue.Load();
                mnode.UpdateAppearance();

                DisplayModel((Model)mnode.MeshNode.DataValue);
            }
            break;
            case ModelNode:
            {
                var mnode = (ModelNode)e.Node;

                if (mnode.DataValue.NeedLoad)
                    mnode.DataValue.Load();
                mnode.UpdateAppearance();

                DisplayModel((Model)mnode.DataValue);
            }
            break;
            case TextureNode:
            {
                var tnode = (TextureNode)e.Node;

                if (tnode.DataValue.NeedLoad)
                    tnode.DataValue.Load();
                tnode.UpdateAppearance();

                DisplayTexture((Texture)tnode.DataValue);
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
                DisplayScenarioVariant(vnode.DataValue);
            }
            break;
            case ScenarioVariantNode:
            {
                var snode = (ScenarioNode)e.Node.Parent;
                var vnode = (ScenarioVariantNode)e.Node;

                if (vnode.DataValue.NeedLoad)
                    vnode.DataValue.Load();
                vnode.UpdateAppearance();
                DisplayScenarioVariant(vnode.DataValue);
            }
            break;
        }

        //dataTreeView.EndUpdate();
    }

    private void DisplayScenarioVariant(ScenarioVariant scenario)
    {
        scene.ClearScene();

        scene.Terrain = scenario.Terrain;

        RenderFrame();
    }

    private void DisplayTexture(Texture texture)
    {
        scene.ClearScene();


        scene.Sprites.Add(new Sprite(texture));

        RenderFrame();
    }

    private void DisplayModel(Model mesh)
    {
        scene.ClearScene();

        scene.Instances.Add(new ModelInstance(mesh));

        RenderFrame();
    }

    private void dataTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
    {
        if (e.Node is DataTreeNode)
        {
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
        }
    }

    private void detectGamesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var games = FindNewGames();
        ShowApplyGamesDialog(games);
    }

    public void RefreshGamesTree()
    {
        List<GameDirectoryNode> listToAdd = new();
        List<GameDirectoryNode> listToRemove = new();

        foreach (var node in dataTreeView.Nodes)
        {
            var gnode = (GameDirectoryNode)node;
            if (!Games.Contains(gnode.DataValue))
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
                if (gnode.DataValue == game)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                var node = new GameDirectoryNode(game);
                node.UpdateAppearance();
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
            if (Games.Contains(game))
            {
                foreach (var node in dataTreeView.Nodes)
                {
                    var gfnode = (GameDirectoryNode)node;
                    if (gfnode.DataValue == game)
                    {
                        gfnode.SeekAndUpdateContent();
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
                sb.AppendLine($"{game.Version} {game.SourcePath}");
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
}
