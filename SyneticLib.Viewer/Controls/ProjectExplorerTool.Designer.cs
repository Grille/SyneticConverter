namespace SyneticLib.WinForms.Controls
{
    partial class ProjectExplorerTool
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
            SuspendLayout();
            // 
            // darkTreeView1
            // 
            darkTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            darkTreeView1.Location = new System.Drawing.Point(0, 25);
            darkTreeView1.MaxDragChange = 20;
            darkTreeView1.Name = "darkTreeView1";
            darkTreeView1.ShowIcons = true;
            darkTreeView1.Size = new System.Drawing.Size(400, 575);
            darkTreeView1.TabIndex = 0;
            darkTreeView1.Text = "darkTreeView1";
            // 
            // ProjectExplorerTool
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(darkTreeView1);
            DefaultDockArea = DarkUI.Docking.DarkDockArea.Left;
            DockText = "Project Explorer";
            Name = "ProjectExplorerTool";
            Size = new System.Drawing.Size(400, 600);
            ResumeLayout(false);
        }

        #endregion

        private DarkUI.Controls.DarkTreeView darkTreeView1;
    }
}
