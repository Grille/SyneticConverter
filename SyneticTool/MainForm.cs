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
    Config config;
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

        config = new("config.dat");
        config.TryLoad();

        Games = config.Games;

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
        var dialog = new AddGameDialog(Games);
        dialog.ShowDialog(this);

        if (dialog.DialogResult == DialogResult.OK)
        {
            dialog.ApplyToGame();
            var game = dialog.SelectedGame;
            if (Games.Contains(game))
            {
                foreach (var node in dataTreeView.Nodes)
                {
                    var gfnode = (GameFolderNode)node;
                    if (gfnode.DataValue == game)
                    {
                        gfnode.SeekAndUpdateContent();
                    }
                }
            }
            else
            {
                Games.Add(game);

                dataTreeView.BeginUpdate();

                var node = new GameFolderNode(game);
                dataTreeView.Nodes.Add(node);
                node.UpdateAppearance();

                dataTreeView.EndUpdate();
            }

            config.Save();
        }
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
                foreach (var name in names)
                {
                    var path = Path.Join(drive.Name, location, name);
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

        glControl.MakeCurrent();

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

            dataTreeView.BeginUpdate();
            foreach (var game in Games)
            {
                var node = new GameFolderNode(game);
                dataTreeView.Nodes.Add(node);
                node.UpdateAppearance();
            }
            dataTreeView.EndUpdate();
        }
    }
}
