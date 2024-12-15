using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DarkUI.Forms;

using SyneticLib.Graphics;
using SyneticLib.IO;
using SyneticLib.WinForms.Controls;
using SyneticLib.WinForms.Resources;

namespace SyneticLib.WinForms.Forms
{
    public partial class EditorForm : DarkForm, ISceneProvider
    {
        readonly ExplorerTool ProjectExplorerToolInstance;

        public EditorForm()
        {
            InitializeComponent();

            AppSettings.Setup();

            Icon = EmbeddedImageList.SyneticLib.Icon;

            Application.AddMessageFilter(DockPanel.DockContentDragFilter);
            Application.AddMessageFilter(DockPanel.DockResizeFilter);

            ProjectExplorerToolInstance = new()
            {
                Owner = this,
            };

            DockPanel.AddContent(ProjectExplorerToolInstance);
            DockPanel.AddContent(new ViewerDocument());

            DockPanel.ContentRemoved += DockPanel_ContentRemoved;
        }

        private void DockPanel_ContentRemoved(object? sender, DarkUI.Docking.DockContentEventArgs e)
        {
            var content = e.Content;

            if (content == ProjectExplorerToolInstance)
            {
                return;
            }

            content.Dispose();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            AppSettings.Save();

            base.OnClosing(e);
        }

        private void explorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DockPanel.AddContent(ProjectExplorerToolInstance);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void viewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DockPanel.AddContent(new ViewerDocument());
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var doc = new ViewerDocument();
            DockPanel.AddContent(doc);
            var load = new SceneLoader(doc, doc.ViewerControl.Scene);
            load.LoadFile();
            doc.DockText = load.FileName;
        }

        public void OpenFile(string filename)
        {
            var doc = new ViewerDocument();
            DockPanel.AddContent(doc);
            var load = new SceneLoader(doc, doc.ViewerControl.Scene);
            load.LoadFile(filename);
            doc.DockText = load.FileName;
        }

        public void GetActiveScene()
        {

        }

        public GlScene GetScene()

        {
            return null!;
        }
    }
}
