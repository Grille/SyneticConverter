using System.Windows.Forms;

namespace SyneticPipelineTool
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
            this.buttonDownP = new System.Windows.Forms.Button();
            this.buttonUpP = new System.Windows.Forms.Button();
            this.buttonCopyP = new System.Windows.Forms.Button();
            this.buttonExecuteP = new System.Windows.Forms.Button();
            this.buttonRemoveP = new System.Windows.Forms.Button();
            this.buttonEditP = new System.Windows.Forms.Button();
            this.buttonNewP = new System.Windows.Forms.Button();
            this.pipelinesListBox = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonDownT = new System.Windows.Forms.Button();
            this.buttonUpT = new System.Windows.Forms.Button();
            this.buttonCopyT = new System.Windows.Forms.Button();
            this.buttonRemoveT = new System.Windows.Forms.Button();
            this.buttonEditT = new System.Windows.Forms.Button();
            this.buttonNewT = new System.Windows.Forms.Button();
            this.tasksListBox = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.buttonDownP);
            this.groupBox1.Controls.Add(this.buttonUpP);
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
            // buttonDownP
            // 
            this.buttonDownP.Location = new System.Drawing.Point(6, 109);
            this.buttonDownP.Name = "buttonDownP";
            this.buttonDownP.Size = new System.Drawing.Size(75, 23);
            this.buttonDownP.TabIndex = 7;
            this.buttonDownP.Text = "Down";
            this.buttonDownP.UseVisualStyleBackColor = true;
            this.buttonDownP.Click += new System.EventHandler(this.buttonDownP_Click);
            // 
            // buttonUpP
            // 
            this.buttonUpP.Location = new System.Drawing.Point(6, 80);
            this.buttonUpP.Name = "buttonUpP";
            this.buttonUpP.Size = new System.Drawing.Size(75, 23);
            this.buttonUpP.TabIndex = 6;
            this.buttonUpP.Text = "Up";
            this.buttonUpP.UseVisualStyleBackColor = true;
            this.buttonUpP.Click += new System.EventHandler(this.buttonUpP_Click);
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
            // buttonExecuteP
            // 
            this.buttonExecuteP.Location = new System.Drawing.Point(87, 51);
            this.buttonExecuteP.Name = "buttonExecuteP";
            this.buttonExecuteP.Size = new System.Drawing.Size(156, 23);
            this.buttonExecuteP.TabIndex = 4;
            this.buttonExecuteP.Text = "Execute";
            this.buttonExecuteP.UseVisualStyleBackColor = true;
            this.buttonExecuteP.Click += new System.EventHandler(this.buttonExecuteP_Click);
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
            this.pipelinesListBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.pipelinesListBox.FormattingEnabled = true;
            this.pipelinesListBox.IntegralHeight = false;
            this.pipelinesListBox.ItemHeight = 14;
            this.pipelinesListBox.Location = new System.Drawing.Point(6, 139);
            this.pipelinesListBox.Name = "pipelinesListBox";
            this.pipelinesListBox.Size = new System.Drawing.Size(236, 298);
            this.pipelinesListBox.TabIndex = 0;
            this.pipelinesListBox.SelectedIndexChanged += new System.EventHandler(this.pipelinesListBox_SelectedIndexChanged);
            this.pipelinesListBox.DoubleClick += new System.EventHandler(this.pipelinesListBox_DoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.buttonDownT);
            this.groupBox2.Controls.Add(this.buttonUpT);
            this.groupBox2.Controls.Add(this.buttonCopyT);
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
            // buttonDownT
            // 
            this.buttonDownT.Location = new System.Drawing.Point(411, 22);
            this.buttonDownT.Name = "buttonDownT";
            this.buttonDownT.Size = new System.Drawing.Size(75, 23);
            this.buttonDownT.TabIndex = 8;
            this.buttonDownT.Text = "Down";
            this.buttonDownT.UseVisualStyleBackColor = true;
            this.buttonDownT.Click += new System.EventHandler(this.buttonDownT_Click);
            // 
            // buttonUpT
            // 
            this.buttonUpT.Location = new System.Drawing.Point(330, 22);
            this.buttonUpT.Name = "buttonUpT";
            this.buttonUpT.Size = new System.Drawing.Size(75, 23);
            this.buttonUpT.TabIndex = 8;
            this.buttonUpT.Text = "Up";
            this.buttonUpT.UseVisualStyleBackColor = true;
            this.buttonUpT.Click += new System.EventHandler(this.buttonUpT_Click);
            // 
            // buttonCopyT
            // 
            this.buttonCopyT.Location = new System.Drawing.Point(249, 22);
            this.buttonCopyT.Name = "buttonCopyT";
            this.buttonCopyT.Size = new System.Drawing.Size(75, 23);
            this.buttonCopyT.TabIndex = 7;
            this.buttonCopyT.Text = "Copy";
            this.buttonCopyT.UseVisualStyleBackColor = true;
            this.buttonCopyT.Click += new System.EventHandler(this.buttonCopyT_Click);
            // 
            // buttonRemoveT
            // 
            this.buttonRemoveT.Location = new System.Drawing.Point(168, 22);
            this.buttonRemoveT.Name = "buttonRemoveT";
            this.buttonRemoveT.Size = new System.Drawing.Size(75, 23);
            this.buttonRemoveT.TabIndex = 6;
            this.buttonRemoveT.Text = "Remove";
            this.buttonRemoveT.UseVisualStyleBackColor = true;
            this.buttonRemoveT.Click += new System.EventHandler(this.buttonRemoveT_Click);
            // 
            // buttonEditT
            // 
            this.buttonEditT.Location = new System.Drawing.Point(87, 22);
            this.buttonEditT.Name = "buttonEditT";
            this.buttonEditT.Size = new System.Drawing.Size(75, 23);
            this.buttonEditT.TabIndex = 5;
            this.buttonEditT.Text = "Edit";
            this.buttonEditT.UseVisualStyleBackColor = true;
            this.buttonEditT.Click += new System.EventHandler(this.buttonEditT_Click);
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
            this.tasksListBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tasksListBox.FormattingEnabled = true;
            this.tasksListBox.IntegralHeight = false;
            this.tasksListBox.ItemHeight = 14;
            this.tasksListBox.Location = new System.Drawing.Point(6, 51);
            this.tasksListBox.Name = "tasksListBox";
            this.tasksListBox.Size = new System.Drawing.Size(511, 386);
            this.tasksListBox.TabIndex = 0;
            this.tasksListBox.SelectedIndexChanged += new System.EventHandler(this.tasksListBox_SelectedIndexChanged);
            this.tasksListBox.DoubleClick += new System.EventHandler(this.tasksListBox_DoubleClick);
            // 
            // SynPipelineToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 467);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Name = "SynPipelineToolForm";
            this.Text = "Synetic Pipeline Tool";
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
        private System.Windows.Forms.Button buttonDownP;
        private System.Windows.Forms.Button buttonUpP;
        private System.Windows.Forms.Button buttonDownT;
        private System.Windows.Forms.Button buttonUpT;
        private System.Windows.Forms.Button buttonCopyT;
    }
}