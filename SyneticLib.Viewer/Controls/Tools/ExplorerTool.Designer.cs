namespace SyneticLib.WinForms.Controls
{
    partial class ExplorerTool
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            darkTreeView1 = new DarkUI.Controls.DarkTreeView();
            darkToolStrip1 = new DarkUI.Controls.DarkToolStrip();
            toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            darkToolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // darkTreeView1
            // 
            darkTreeView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            darkTreeView1.Location = new System.Drawing.Point(0, 56);
            darkTreeView1.MaxDragChange = 20;
            darkTreeView1.Name = "darkTreeView1";
            darkTreeView1.ShowIcons = true;
            darkTreeView1.Size = new System.Drawing.Size(400, 544);
            darkTreeView1.TabIndex = 0;
            darkTreeView1.Text = "darkTreeView1";
            // 
            // darkToolStrip1
            // 
            darkToolStrip1.AutoSize = false;
            darkToolStrip1.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            darkToolStrip1.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            darkToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButton1, toolStripButton2, toolStripButton3 });
            darkToolStrip1.Location = new System.Drawing.Point(0, 25);
            darkToolStrip1.Name = "darkToolStrip1";
            darkToolStrip1.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
            darkToolStrip1.Size = new System.Drawing.Size(400, 28);
            darkToolStrip1.TabIndex = 1;
            darkToolStrip1.Text = "darkToolStrip1";
            // 
            // toolStripButton1
            // 
            toolStripButton1.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            toolStripButton1.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripButton1.Image = Properties.Resources.Add;
            toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new System.Drawing.Size(49, 25);
            toolStripButton1.Text = "Add";
            // 
            // toolStripButton2
            // 
            toolStripButton2.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            toolStripButton2.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripButton2.Image = Properties.Resources.Remove;
            toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new System.Drawing.Size(70, 25);
            toolStripButton2.Text = "Remove";
            // 
            // toolStripButton3
            // 
            toolStripButton3.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            toolStripButton3.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripButton3.Image = Properties.Resources.Search;
            toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton3.Name = "toolStripButton3";
            toolStripButton3.Size = new System.Drawing.Size(62, 25);
            toolStripButton3.Text = "Search";
            // 
            // ExplorerTool
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(darkToolStrip1);
            Controls.Add(darkTreeView1);
            DefaultDockArea = DarkUI.Docking.DarkDockArea.Left;
            DockText = "Project Explorer";
            Name = "ExplorerTool";
            Size = new System.Drawing.Size(400, 600);
            darkToolStrip1.ResumeLayout(false);
            darkToolStrip1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DarkUI.Controls.DarkTreeView darkTreeView1;
        private DarkUI.Controls.DarkToolStrip darkToolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
    }
}
