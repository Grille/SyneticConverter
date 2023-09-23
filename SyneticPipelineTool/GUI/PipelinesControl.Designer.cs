namespace SyneticPipelineTool.GUI
{
    partial class PipelinesControl
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
            components = new System.ComponentModel.Container();
            toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            ListBox = new PipelineListBox();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            upToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            downToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            ButtonNew = new System.Windows.Forms.ToolStripButton();
            ButtonEdit = new System.Windows.Forms.ToolStripButton();
            ButtonRemove = new System.Windows.Forms.ToolStripButton();
            ButtonCopy = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            ButtonUp = new System.Windows.Forms.ToolStripButton();
            ButtonDown = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            ButtonRun = new System.Windows.Forms.ToolStripButton();
            ButtonStop = new System.Windows.Forms.ToolStripButton();
            refreshTimer = new System.Windows.Forms.Timer(components);
            toolStripContainer1.ContentPanel.SuspendLayout();
            toolStripContainer1.TopToolStripPanel.SuspendLayout();
            toolStripContainer1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            toolStripContainer1.ContentPanel.Controls.Add(ListBox);
            toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(500, 475);
            toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            toolStripContainer1.Name = "toolStripContainer1";
            toolStripContainer1.Size = new System.Drawing.Size(500, 500);
            toolStripContainer1.TabIndex = 0;
            toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            toolStripContainer1.TopToolStripPanel.Controls.Add(toolStrip1);
            // 
            // ListBox
            // 
            ListBox.ContextMenuStrip = contextMenuStrip1;
            ListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            ListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            ListBox.Executer = null;
            ListBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            ListBox.FormattingEnabled = true;
            ListBox.IntegralHeight = false;
            ListBox.Location = new System.Drawing.Point(0, 0);
            ListBox.Name = "ListBox";
            ListBox.SelectedItem = null;
            ListBox.Size = new System.Drawing.Size(500, 475);
            ListBox.TabIndex = 0;
            ListBox.SelectedIndexChanged += ListBoxIndexChanged;
            ListBox.DoubleClick += ListBoxDoubleClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { newToolStripMenuItem, editToolStripMenuItem, deleteToolStripMenuItem, copyToolStripMenuItem, toolStripSeparator3, upToolStripMenuItem, downToolStripMenuItem, toolStripSeparator4, runToolStripMenuItem, stopToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(108, 192);
            contextMenuStrip1.Text = "Edit";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Image = Properties.Resources.New;
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            newToolStripMenuItem.Text = "New";
            newToolStripMenuItem.Click += NewClick;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Image = Properties.Resources.Edit;
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            editToolStripMenuItem.Text = "Edit";
            editToolStripMenuItem.Click += EditClick;
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Image = Properties.Resources.Delete;
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            deleteToolStripMenuItem.Text = "Delete";
            deleteToolStripMenuItem.Click += DeleteClick;
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Image = Properties.Resources.Copy;
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += CopyClick;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(104, 6);
            // 
            // upToolStripMenuItem
            // 
            upToolStripMenuItem.Image = Properties.Resources.MoveUp;
            upToolStripMenuItem.Name = "upToolStripMenuItem";
            upToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            upToolStripMenuItem.Text = "Up";
            upToolStripMenuItem.Click += UpClick;
            // 
            // downToolStripMenuItem
            // 
            downToolStripMenuItem.Image = Properties.Resources.MoveDown;
            downToolStripMenuItem.Name = "downToolStripMenuItem";
            downToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            downToolStripMenuItem.Text = "Down";
            downToolStripMenuItem.Click += DownClick;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(104, 6);
            // 
            // runToolStripMenuItem
            // 
            runToolStripMenuItem.Image = Properties.Resources.Run;
            runToolStripMenuItem.Name = "runToolStripMenuItem";
            runToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            runToolStripMenuItem.Text = "Run";
            runToolStripMenuItem.Click += RunClick;
            // 
            // stopToolStripMenuItem
            // 
            stopToolStripMenuItem.Image = Properties.Resources.Stop;
            stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            stopToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            stopToolStripMenuItem.Text = "Stop";
            stopToolStripMenuItem.Click += StopClick;
            // 
            // toolStrip1
            // 
            toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ButtonNew, ButtonEdit, ButtonRemove, ButtonCopy, toolStripSeparator1, ButtonUp, ButtonDown, toolStripSeparator2, ButtonRun, ButtonStop });
            toolStrip1.Location = new System.Drawing.Point(3, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(208, 25);
            toolStrip1.TabIndex = 0;
            // 
            // ButtonNew
            // 
            ButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ButtonNew.Image = Properties.Resources.New;
            ButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            ButtonNew.Name = "ButtonNew";
            ButtonNew.Size = new System.Drawing.Size(23, 22);
            ButtonNew.Text = "New";
            ButtonNew.Click += NewClick;
            // 
            // ButtonEdit
            // 
            ButtonEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ButtonEdit.Image = Properties.Resources.Edit;
            ButtonEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            ButtonEdit.Name = "ButtonEdit";
            ButtonEdit.Size = new System.Drawing.Size(23, 22);
            ButtonEdit.Text = "Edit";
            ButtonEdit.Click += EditClick;
            // 
            // ButtonRemove
            // 
            ButtonRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ButtonRemove.Image = Properties.Resources.Delete;
            ButtonRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            ButtonRemove.Name = "ButtonRemove";
            ButtonRemove.Size = new System.Drawing.Size(23, 22);
            ButtonRemove.Text = "Delete";
            ButtonRemove.Click += DeleteClick;
            // 
            // ButtonCopy
            // 
            ButtonCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ButtonCopy.Image = Properties.Resources.Copy;
            ButtonCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            ButtonCopy.Name = "ButtonCopy";
            ButtonCopy.Size = new System.Drawing.Size(23, 22);
            ButtonCopy.Text = "Copy";
            ButtonCopy.Click += CopyClick;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonUp
            // 
            ButtonUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ButtonUp.Image = Properties.Resources.MoveUp;
            ButtonUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            ButtonUp.Name = "ButtonUp";
            ButtonUp.Size = new System.Drawing.Size(23, 22);
            ButtonUp.Text = "Up";
            ButtonUp.Click += UpClick;
            // 
            // ButtonDown
            // 
            ButtonDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ButtonDown.Image = Properties.Resources.MoveDown;
            ButtonDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            ButtonDown.Name = "ButtonDown";
            ButtonDown.Size = new System.Drawing.Size(23, 22);
            ButtonDown.Text = "Down";
            ButtonDown.Click += DownClick;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonRun
            // 
            ButtonRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ButtonRun.Image = Properties.Resources.Run;
            ButtonRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            ButtonRun.Name = "ButtonRun";
            ButtonRun.Size = new System.Drawing.Size(23, 22);
            ButtonRun.Text = "Run";
            ButtonRun.Click += RunClick;
            // 
            // ButtonStop
            // 
            ButtonStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ButtonStop.Enabled = false;
            ButtonStop.Image = Properties.Resources.Stop;
            ButtonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            ButtonStop.Name = "ButtonStop";
            ButtonStop.Size = new System.Drawing.Size(23, 22);
            ButtonStop.Text = "Stop";
            ButtonStop.Click += StopClick;
            // 
            // refreshTimer
            // 
            refreshTimer.Tick += TimerTick;
            // 
            // PipelinesControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(toolStripContainer1);
            DoubleBuffered = true;
            Name = "PipelinesControl";
            Size = new System.Drawing.Size(500, 500);
            Load += ControlLoad;
            toolStripContainer1.ContentPanel.ResumeLayout(false);
            toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            toolStripContainer1.TopToolStripPanel.PerformLayout();
            toolStripContainer1.ResumeLayout(false);
            toolStripContainer1.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ButtonNew;
        private System.Windows.Forms.ToolStripButton ButtonEdit;
        private System.Windows.Forms.ToolStripButton ButtonRemove;
        private System.Windows.Forms.ToolStripButton ButtonDown;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ButtonUp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton ButtonRun;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem upToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton ButtonCopy;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.Timer refreshTimer;
        public PipelineListBox ListBox;
        private System.Windows.Forms.ToolStripButton ButtonStop;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
    }
}
