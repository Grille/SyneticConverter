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


using SyneticLib;
using SyneticLib.Graphics;

using SyneticTool.Nodes;

namespace SyneticTool;

public partial class MainForm : Form
{
    public Games Games;
    GLControl glControl;
    Config config;
    Mesh mesh;
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
        GLSyn.Viewport(0, 0, glControl.ClientSize.Width, glControl.ClientSize.Height);
        errorPanel.Location = new Point(glControl.ClientSize.Width / 2 - errorPanel.Width / 2, glControl.ClientSize.Height / 2 - errorPanel.Height / 2);
    }

    private void addGameToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var dialog = new AddGameDialog(Games);
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
        Games.AddGame("WR1", "X:/Games/Synetic/World Racing", GameVersion.MBWR);
        Games.AddGame("WR2", "C:/World Racing 2", GameVersion.WR2);
        Games.AddGame("C11", "X:/Games/Synetic/Cobra 11 - Nitro", GameVersion.C11);
        Games.AddGame("CT1", "X:/Games/Synetic/Cobra 11 - Crash Time", GameVersion.CTP);
        Games.AddGame("CT2", "X:/Games/Synetic/Cobra 11 - Burning Wheels", GameVersion.CT2);
        Games.AddGame("CT3", "X:/Games/Synetic/Cobra 11 - Highway Nights", GameVersion.CT3);
        Games.AddGame("CT4", "X:/Games/Synetic/Cobra 11 - Das Syndikat", GameVersion.CT4);
        Games.AddGame("CT5", "X:/Games/Synetic/Cobra 11 - Undercover", GameVersion.CT5);
        Games.AddGame("FVR", "X:/Games/Synetic/Ferrari Virtual Race", GameVersion.FVR);

        UpdateTree();

        glControl.MakeCurrent();
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
        Console.WriteLine("next call");
        GLSyn.Clear();

        if (mesh != null)
            GLSyn.DrawMesh(mesh);

        Console.WriteLine("swap");
        glControl.SwapBuffers();
    }

    private void dataTreeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
        switch (e.Node)
        {
            case ScenarioNode:
            {
                var snode = (ScenarioNode)e.Node;
                var vnode = snode.V1;

                DisplayScenarioVariant(snode.Value.V1);

                snode.UpdateColor();
                vnode?.UpdateColor();
            }
            break;
            case ScenarioVariantNode:
            {
                var snode = (ScenarioNode)e.Node.Parent;
                var vnode = (ScenarioVariantNode)e.Node;

                DisplayScenarioVariant(vnode.Value);

                snode.UpdateColor();
                vnode.UpdateColor();
            }
            break;
        }
    }

    private void DisplayScenarioVariant(ScenarioVariant v)
    {

    }

    private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
    {

    }
}
