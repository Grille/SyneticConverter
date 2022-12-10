namespace SyneticBasicTools
{
    partial class SynPipelineToolForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonExecuteP = new System.Windows.Forms.Button();
            this.buttonRemoveP = new System.Windows.Forms.Button();
            this.buttonEditP = new System.Windows.Forms.Button();
            this.buttonNewP = new System.Windows.Forms.Button();
            this.pipelinesListBox = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonRemoveT = new System.Windows.Forms.Button();
            this.buttonEditT = new System.Windows.Forms.Button();
            this.buttonNewT = new System.Windows.Forms.Button();
            this.tasksListBox = new System.Windows.Forms.ListBox();
            this.buttonCopyP = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.buttonCopyP);
            this.groupBox1.Controls.Add(this.buttonExecuteP);
            this.groupBox1.Controls.Add(this.buttonRemoveP);
            this.groupBox1.Controls.Add(this.buttonEditP);
            this.groupBox1.Controls.Add(this.buttonNewP);
            this.groupBox1.Controls.Add(this.pipelinesListBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 443);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pipelines";
            // 
            // buttonExecuteP
            // 
            this.buttonExecuteP.Location = new System.Drawing.Point(87, 51);
            this.buttonExecuteP.Name = "buttonExecuteP";
            this.buttonExecuteP.Size = new System.Drawing.Size(156, 23);
            this.buttonExecuteP.TabIndex = 4;
            this.buttonExecuteP.Text = "Execute";
            this.buttonExecuteP.UseVisualStyleBackColor = true;
            // 
            // buttonRemoveP
            // 
            this.buttonRemoveP.Location = new System.Drawing.Point(168, 22);
            this.buttonRemoveP.Name = "buttonRemoveP";
            this.buttonRemoveP.Size = new System.Drawing.Size(75, 23);
            this.buttonRemoveP.TabIndex = 3;
            this.buttonRemoveP.Text = "Remove";
            this.buttonRemoveP.UseVisualStyleBackColor = true;
            this.buttonRemoveP.Click += new System.EventHandler(this.buttonRemoveP_Click);
            // 
            // buttonEditP
            // 
            this.buttonEditP.Location = new System.Drawing.Point(87, 22);
            this.buttonEditP.Name = "buttonEditP";
            this.buttonEditP.Size = new System.Drawing.Size(75, 23);
            this.buttonEditP.TabIndex = 2;
            this.buttonEditP.Text = "Edit";
            this.buttonEditP.UseVisualStyleBackColor = true;
            this.buttonEditP.Click += new System.EventHandler(this.buttonEditP_Click);
            // 
            // buttonNewP
            // 
            this.buttonNewP.Location = new System.Drawing.Point(6, 22);
            this.buttonNewP.Name = "buttonNewP";
            this.buttonNewP.Size = new System.Drawing.Size(75, 23);
            this.buttonNewP.TabIndex = 1;
            this.buttonNewP.Text = "New";
            this.buttonNewP.UseVisualStyleBackColor = true;
            this.buttonNewP.Click += new System.EventHandler(this.buttonNewP_Click);
            // 
            // pipelinesListBox
            // 
            this.pipelinesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pipelinesListBox.FormattingEnabled = true;
            this.pipelinesListBox.IntegralHeight = false;
            this.pipelinesListBox.ItemHeight = 15;
            this.pipelinesListBox.Location = new System.Drawing.Point(6, 88);
            this.pipelinesListBox.Name = "pipelinesListBox";
            this.pipelinesListBox.Size = new System.Drawing.Size(236, 349);
            this.pipelinesListBox.TabIndex = 0;
            this.pipelinesListBox.SelectedIndexChanged += new System.EventHandler(this.pipelinesListBox_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.buttonRemoveT);
            this.groupBox2.Controls.Add(this.buttonEditT);
            this.groupBox2.Controls.Add(this.buttonNewT);
            this.groupBox2.Controls.Add(this.tasksListBox);
            this.groupBox2.Location = new System.Drawing.Point(266, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(523, 443);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tasks";
            // 
            // buttonRemoveT
            // 
            this.buttonRemoveT.Location = new System.Drawing.Point(168, 22);
            this.buttonRemoveT.Name = "buttonRemoveT";
            this.buttonRemoveT.Size = new System.Drawing.Size(75, 23);
            this.buttonRemoveT.TabIndex = 6;
            this.buttonRemoveT.Text = "Remove";
            this.buttonRemoveT.UseVisualStyleBackColor = true;
            // 
            // buttonEditT
            // 
            this.buttonEditT.Location = new System.Drawing.Point(87, 22);
            this.buttonEditT.Name = "buttonEditT";
            this.buttonEditT.Size = new System.Drawing.Size(75, 23);
            this.buttonEditT.TabIndex = 5;
            this.buttonEditT.Text = "Edit";
            this.buttonEditT.UseVisualStyleBackColor = true;
            // 
            // buttonNewT
            // 
            this.buttonNewT.Location = new System.Drawing.Point(6, 22);
            this.buttonNewT.Name = "buttonNewT";
            this.buttonNewT.Size = new System.Drawing.Size(75, 23);
            this.buttonNewT.TabIndex = 2;
            this.buttonNewT.Text = "New";
            this.buttonNewT.UseVisualStyleBackColor = true;
            this.buttonNewT.Click += new System.EventHandler(this.buttonNewT_Click);
            // 
            // tasksListBox
            // 
            this.tasksListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tasksListBox.FormattingEnabled = true;
            this.tasksListBox.IntegralHeight = false;
            this.tasksListBox.ItemHeight = 15;
            this.tasksListBox.Location = new System.Drawing.Point(6, 51);
            this.tasksListBox.Name = "tasksListBox";
            this.tasksListBox.Size = new System.Drawing.Size(511, 386);
            this.tasksListBox.TabIndex = 0;
            // 
            // buttonCopyP
            // 
            this.buttonCopyP.Location = new System.Drawing.Point(6, 51);
            this.buttonCopyP.Name = "buttonCopyP";
            this.buttonCopyP.Size = new System.Drawing.Size(75, 23);
            this.buttonCopyP.TabIndex = 5;
            this.buttonCopyP.Text = "Copy";
            this.buttonCopyP.UseVisualStyleBackColor = true;
            this.buttonCopyP.Click += new System.EventHandler(this.buttonCopyP_Click);
            // 
            // SynPipelineToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 467);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "SynPipelineToolForm";
            this.Text = "SynPipelineToolForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox pipelinesListBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox tasksListBox;
        private System.Windows.Forms.Button buttonRemoveP;
        private System.Windows.Forms.Button buttonEditP;
        private System.Windows.Forms.Button buttonNewP;
        private System.Windows.Forms.Button buttonExecuteP;
        private System.Windows.Forms.Button buttonRemoveT;
        private System.Windows.Forms.Button buttonEditT;
        private System.Windows.Forms.Button buttonNewT;
        private System.Windows.Forms.Button buttonCopyP;
    }
}