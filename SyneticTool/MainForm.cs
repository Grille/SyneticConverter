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
    public Games Games;
    GLControl glControl;
    Config config;
    Scene scene;

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

        glControl.Resize += GlControl_Resize;
        dataTreeView.ImageList = IconList.Images;

        config = new Config("config.ini");

        if (!config.Exists())
        {
            config.Save();
        }

        config.Load();

        Games = new Games();

        errorPanel.Visible = false;
        errorPanel.BringToFront();

        DoubleBuffered = true;

        scene = new();
    }

    private void GlControl_Paint(object sender, PaintEventArgs e)
    {
        RenderFrame();
    }

    private void GlControl_Resize(object sender, EventArgs e)
    {
        errorPanel.Location = new Point(glControl.ClientSize.Width / 2 - errorPanel.Width / 2, glControl.ClientSize.Height / 2 - errorPanel.Height / 2);
        scene.Camera.ScreenSize = new OpenTK.Mathematics.Vector2(glControl.ClientSize.Width, glControl.ClientSize.Height);
        scene.Camera.CreatePerspective();
    }

    private void addGameToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var dialog = new AddGameDialog(Games);
        dialog.ShowDialog(this);

        if (dialog.DialogResult == DialogResult.OK)
        {
            if (!Games.Exists(dialog.GameName))
            {
                var game = Games.CreateGame(dialog.GameName, dialog.GamePath);
                var node = new GameFolderNode(game);
                node.UpdateAppearance();
                dataTreeView.Nodes.Add(node);
            }
        }
    }

    private void convertToToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }



    private void MainForm_Shown(object sender, EventArgs e)
    {
        Games.CreateGame("WR1", "X:/Games/Synetic/World Racing", GameVersion.MBWR);
        Games.CreateGame("WR2", "C:/World Racing 2", GameVersion.WR2);
        Games.CreateGame("C11", "X:/Games/Synetic/Cobra 11 - Nitro", GameVersion.C11);
        Games.CreateGame("CT1", "X:/Games/Synetic/Cobra 11 - Crash Time", GameVersion.CTP);
        Games.CreateGame("CT2", "X:/Games/Synetic/Cobra 11 - Burning Wheels", GameVersion.CT2);
        Games.CreateGame("CT3", "X:/Games/Synetic/Cobra 11 - Highway Nights", GameVersion.CT3);
        Games.CreateGame("CT4", "X:/Games/Synetic/Cobra 11 - Das Syndikat", GameVersion.CT4);
        Games.CreateGame("CT5", "X:/Games/Synetic/Cobra 11 - Undercover", GameVersion.CT5);
        Games.CreateGame("FVR", "X:/Games/Synetic/Ferrari Virtual Race", GameVersion.FVR);

        dataTreeView.BeginUpdate();
        foreach (var game in Games.GameFolders)
        {
            var node = new GameFolderNode(game.Value);
            dataTreeView.Nodes.Add(node);
            node.UpdateAppearance();
        }
        dataTreeView.EndUpdate();

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
        glControl.MakeCurrent();

        scene.ClearScreen();
        scene.Render();

        glControl.SwapBuffers();
    }

    private void dataTreeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
        switch (e.Node)
        {
            case MeshNode:
            {
                var mnode = (MeshNode)e.Node;

                mnode.DataValue.Load();
                mnode.UpdateAppearance();

                DisplayMesh((Mesh)mnode.DataValue);
            }
            break;
            case TextureNode:
            {
                var tnode = (TextureNode)e.Node;

                tnode.DataValue.Load();
                tnode.UpdateAppearance();

                DisplayTexture((Texture)tnode.DataValue);
            }
            break;
            case ScenarioNode:
            {
                var snode = (ScenarioNode)e.Node;
                var vnode = snode.V1;

                vnode.DataValue.Load();
                vnode.UpdateAppearance();
                DisplayScenarioVariant(vnode.DataValue);
            }
            break;
            case ScenarioVariantNode:
            {
                var snode = (ScenarioNode)e.Node.Parent;
                var vnode = (ScenarioVariantNode)e.Node;

                vnode.DataValue.Load();
                vnode.UpdateAppearance();
                DisplayScenarioVariant(vnode.DataValue);
            }
            break;
        }
    }

    private void DisplayScenarioVariant(ScenarioVariant scenario)
    {
        scene.ClearScene();

        RenderFrame();
    }

    private void DisplayTexture(Texture texture)
    {
        scene.ClearScene();

        scene.Sprites.Add(new Sprite(texture));

        RenderFrame();
    }

    private void DisplayMesh(Mesh mesh)
    {
        scene.ClearScene();

        RenderFrame();
    }

    private void dataTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
    {
        dataTreeView.BeginUpdate();

        if (e.Node is DataTreeNode)
        {
            var node = (DataTreeNode)e.Node;
            foreach (var n in node.Nodes)
            {
                if (!(n is DataTreeNode))
                    continue;

                var cnode = (DataTreeNode)n;
                cnode.SeekAndUpdateContent();
            }
            node.UpdateAppearance();
        }

        dataTreeView.EndUpdate();
    }
}
