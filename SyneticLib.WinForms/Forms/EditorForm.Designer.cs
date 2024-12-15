namespace SyneticLib.WinForms.Forms
{
    partial class EditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DockPanel = new DarkUI.Docking.DarkDockPanel();
            darkToolStrip1 = new DarkUI.Controls.DarkToolStrip();
            darkStatusStrip1 = new DarkUI.Controls.DarkStatusStrip();
            darkMenuStrip1 = new DarkUI.Controls.DarkMenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            explorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            viewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            gHJToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            darkMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // DockPanel
            // 
            DockPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            DockPanel.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            DockPanel.Location = new System.Drawing.Point(0, 52);
            DockPanel.Margin = new System.Windows.Forms.Padding(0);
            DockPanel.Name = "DockPanel";
            DockPanel.Size = new System.Drawing.Size(800, 374);
            DockPanel.TabIndex = 0;
            // 
            // darkToolStrip1
            // 
            darkToolStrip1.AutoSize = false;
            darkToolStrip1.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            darkToolStrip1.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            darkToolStrip1.Location = new System.Drawing.Point(0, 24);
            darkToolStrip1.Name = "darkToolStrip1";
            darkToolStrip1.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
            darkToolStrip1.Size = new System.Drawing.Size(800, 28);
            darkToolStrip1.TabIndex = 1;
            darkToolStrip1.Text = "darkToolStrip1";
            // 
            // darkStatusStrip1
            // 
            darkStatusStrip1.AutoSize = false;
            darkStatusStrip1.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            darkStatusStrip1.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            darkStatusStrip1.Location = new System.Drawing.Point(0, 426);
            darkStatusStrip1.Name = "darkStatusStrip1";
            darkStatusStrip1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 3);
            darkStatusStrip1.Size = new System.Drawing.Size(800, 24);
            darkStatusStrip1.SizingGrip = false;
            darkStatusStrip1.TabIndex = 2;
            darkStatusStrip1.Text = "darkStatusStrip1";
            // 
            // darkMenuStrip1
            // 
            darkMenuStrip1.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            darkMenuStrip1.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            darkMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, viewToolStripMenuItem });
            darkMenuStrip1.Location = new System.Drawing.Point(0, 0);
            darkMenuStrip1.Name = "darkMenuStrip1";
            darkMenuStrip1.Padding = new System.Windows.Forms.Padding(3, 2, 0, 2);
            darkMenuStrip1.Size = new System.Drawing.Size(800, 24);
            darkMenuStrip1.TabIndex = 3;
            darkMenuStrip1.Text = "darkMenuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { openToolStripMenuItem, toolStripSeparator2, exitToolStripMenuItem });
            fileToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            openToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
            openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            toolStripSeparator2.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripSeparator2.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            exitToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q;
            exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { explorerToolStripMenuItem, toolStripSeparator1, viewerToolStripMenuItem, gHJToolStripMenuItem });
            viewToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            viewToolStripMenuItem.Text = "View";
            // 
            // explorerToolStripMenuItem
            // 
            explorerToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            explorerToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            explorerToolStripMenuItem.Name = "explorerToolStripMenuItem";
            explorerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            explorerToolStripMenuItem.Text = "Explorer";
            explorerToolStripMenuItem.Click += explorerToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            toolStripSeparator1.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripSeparator1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // viewerToolStripMenuItem
            // 
            viewerToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            viewerToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            viewerToolStripMenuItem.Name = "viewerToolStripMenuItem";
            viewerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            viewerToolStripMenuItem.Text = "Viewer";
            viewerToolStripMenuItem.Click += viewerToolStripMenuItem_Click;
            // 
            // gHJToolStripMenuItem
            // 
            gHJToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            gHJToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            gHJToolStripMenuItem.Name = "gHJToolStripMenuItem";
            gHJToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            gHJToolStripMenuItem.Text = "GHJ";
            // 
            // EditorForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(darkStatusStrip1);
            Controls.Add(darkToolStrip1);
            Controls.Add(darkMenuStrip1);
            Controls.Add(DockPanel);
            MainMenuStrip = darkMenuStrip1;
            Name = "EditorForm";
            Text = "EditorForm";
            darkMenuStrip1.ResumeLayout(false);
            darkMenuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DarkUI.Docking.DarkDockPanel DockPanel;
        private DarkUI.Controls.DarkToolStrip darkToolStrip1;
        private DarkUI.Controls.DarkStatusStrip darkStatusStrip1;
        private DarkUI.Controls.DarkMenuStrip darkMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem explorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem viewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem gHJToolStripMenuItem;
    }
}