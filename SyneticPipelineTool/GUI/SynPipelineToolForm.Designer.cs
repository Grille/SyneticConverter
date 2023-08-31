using SyneticPipelineTool.GUI;
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
            components = new System.ComponentModel.Container();
            groupBox1 = new GroupBox();
            buttonDownP = new Button();
            buttonUpP = new Button();
            buttonCopyP = new Button();
            buttonExecuteP = new Button();
            buttonRemoveP = new Button();
            buttonEditP = new Button();
            buttonNewP = new Button();
            pipelinesListBox = new PipelineListBox();
            groupBox2 = new GroupBox();
            buttonDownT = new Button();
            buttonUpT = new Button();
            buttonCopyT = new Button();
            buttonRemoveT = new Button();
            buttonEditT = new Button();
            buttonNewT = new Button();
            tasksListBox = new PipelineTaskListBox();
            refreshTimer = new Timer(components);
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            groupBox1.Controls.Add(buttonDownP);
            groupBox1.Controls.Add(buttonUpP);
            groupBox1.Controls.Add(buttonCopyP);
            groupBox1.Controls.Add(buttonExecuteP);
            groupBox1.Controls.Add(buttonRemoveP);
            groupBox1.Controls.Add(buttonEditP);
            groupBox1.Controls.Add(buttonNewP);
            groupBox1.Controls.Add(pipelinesListBox);
            groupBox1.Location = new System.Drawing.Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(248, 443);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Pipelines";
            // 
            // buttonDownP
            // 
            buttonDownP.Location = new System.Drawing.Point(6, 109);
            buttonDownP.Name = "buttonDownP";
            buttonDownP.Size = new System.Drawing.Size(75, 23);
            buttonDownP.TabIndex = 7;
            buttonDownP.Text = "Down";
            buttonDownP.UseVisualStyleBackColor = true;
            buttonDownP.Click += buttonDownP_Click;
            // 
            // buttonUpP
            // 
            buttonUpP.Location = new System.Drawing.Point(6, 80);
            buttonUpP.Name = "buttonUpP";
            buttonUpP.Size = new System.Drawing.Size(75, 23);
            buttonUpP.TabIndex = 6;
            buttonUpP.Text = "Up";
            buttonUpP.UseVisualStyleBackColor = true;
            buttonUpP.Click += buttonUpP_Click;
            // 
            // buttonCopyP
            // 
            buttonCopyP.Location = new System.Drawing.Point(6, 51);
            buttonCopyP.Name = "buttonCopyP";
            buttonCopyP.Size = new System.Drawing.Size(75, 23);
            buttonCopyP.TabIndex = 5;
            buttonCopyP.Text = "Copy";
            buttonCopyP.UseVisualStyleBackColor = true;
            buttonCopyP.Click += buttonCopyP_Click;
            // 
            // buttonExecuteP
            // 
            buttonExecuteP.Location = new System.Drawing.Point(87, 51);
            buttonExecuteP.Name = "buttonExecuteP";
            buttonExecuteP.Size = new System.Drawing.Size(156, 23);
            buttonExecuteP.TabIndex = 4;
            buttonExecuteP.Text = "Execute";
            buttonExecuteP.UseVisualStyleBackColor = true;
            buttonExecuteP.Click += buttonExecuteP_Click;
            // 
            // buttonRemoveP
            // 
            buttonRemoveP.Location = new System.Drawing.Point(168, 22);
            buttonRemoveP.Name = "buttonRemoveP";
            buttonRemoveP.Size = new System.Drawing.Size(75, 23);
            buttonRemoveP.TabIndex = 3;
            buttonRemoveP.Text = "Remove";
            buttonRemoveP.UseVisualStyleBackColor = true;
            buttonRemoveP.Click += buttonRemoveP_Click;
            // 
            // buttonEditP
            // 
            buttonEditP.Location = new System.Drawing.Point(87, 22);
            buttonEditP.Name = "buttonEditP";
            buttonEditP.Size = new System.Drawing.Size(75, 23);
            buttonEditP.TabIndex = 2;
            buttonEditP.Text = "Edit";
            buttonEditP.UseVisualStyleBackColor = true;
            buttonEditP.Click += buttonEditP_Click;
            // 
            // buttonNewP
            // 
            buttonNewP.Location = new System.Drawing.Point(6, 22);
            buttonNewP.Name = "buttonNewP";
            buttonNewP.Size = new System.Drawing.Size(75, 23);
            buttonNewP.TabIndex = 1;
            buttonNewP.Text = "New";
            buttonNewP.UseVisualStyleBackColor = true;
            buttonNewP.Click += buttonNewP_Click;
            // 
            // pipelinesListBox
            // 
            pipelinesListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pipelinesListBox.DrawMode = DrawMode.OwnerDrawFixed;
            pipelinesListBox.Executer = null;
            pipelinesListBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            pipelinesListBox.FormattingEnabled = true;
            pipelinesListBox.IntegralHeight = false;
            pipelinesListBox.ItemHeight = 14;
            pipelinesListBox.Location = new System.Drawing.Point(6, 139);
            pipelinesListBox.Name = "pipelinesListBox";
            pipelinesListBox.SelectedItem = null;
            pipelinesListBox.Size = new System.Drawing.Size(236, 298);
            pipelinesListBox.TabIndex = 0;
            pipelinesListBox.SelectedIndexChanged += pipelinesListBox_SelectedIndexChanged;
            pipelinesListBox.DoubleClick += pipelinesListBox_DoubleClick;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(buttonDownT);
            groupBox2.Controls.Add(buttonUpT);
            groupBox2.Controls.Add(buttonCopyT);
            groupBox2.Controls.Add(buttonRemoveT);
            groupBox2.Controls.Add(buttonEditT);
            groupBox2.Controls.Add(buttonNewT);
            groupBox2.Controls.Add(tasksListBox);
            groupBox2.Location = new System.Drawing.Point(266, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(523, 443);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Tasks";
            // 
            // buttonDownT
            // 
            buttonDownT.Location = new System.Drawing.Point(411, 22);
            buttonDownT.Name = "buttonDownT";
            buttonDownT.Size = new System.Drawing.Size(75, 23);
            buttonDownT.TabIndex = 8;
            buttonDownT.Text = "Down";
            buttonDownT.UseVisualStyleBackColor = true;
            buttonDownT.Click += buttonDownT_Click;
            // 
            // buttonUpT
            // 
            buttonUpT.Location = new System.Drawing.Point(330, 22);
            buttonUpT.Name = "buttonUpT";
            buttonUpT.Size = new System.Drawing.Size(75, 23);
            buttonUpT.TabIndex = 8;
            buttonUpT.Text = "Up";
            buttonUpT.UseVisualStyleBackColor = true;
            buttonUpT.Click += buttonUpT_Click;
            // 
            // buttonCopyT
            // 
            buttonCopyT.Location = new System.Drawing.Point(249, 22);
            buttonCopyT.Name = "buttonCopyT";
            buttonCopyT.Size = new System.Drawing.Size(75, 23);
            buttonCopyT.TabIndex = 7;
            buttonCopyT.Text = "Copy";
            buttonCopyT.UseVisualStyleBackColor = true;
            buttonCopyT.Click += buttonCopyT_Click;
            // 
            // buttonRemoveT
            // 
            buttonRemoveT.Location = new System.Drawing.Point(168, 22);
            buttonRemoveT.Name = "buttonRemoveT";
            buttonRemoveT.Size = new System.Drawing.Size(75, 23);
            buttonRemoveT.TabIndex = 6;
            buttonRemoveT.Text = "Remove";
            buttonRemoveT.UseVisualStyleBackColor = true;
            buttonRemoveT.Click += buttonRemoveT_Click;
            // 
            // buttonEditT
            // 
            buttonEditT.Location = new System.Drawing.Point(87, 22);
            buttonEditT.Name = "buttonEditT";
            buttonEditT.Size = new System.Drawing.Size(75, 23);
            buttonEditT.TabIndex = 5;
            buttonEditT.Text = "Edit";
            buttonEditT.UseVisualStyleBackColor = true;
            buttonEditT.Click += buttonEditT_Click;
            // 
            // buttonNewT
            // 
            buttonNewT.Location = new System.Drawing.Point(6, 22);
            buttonNewT.Name = "buttonNewT";
            buttonNewT.Size = new System.Drawing.Size(75, 23);
            buttonNewT.TabIndex = 2;
            buttonNewT.Text = "New";
            buttonNewT.UseVisualStyleBackColor = true;
            buttonNewT.Click += buttonNewT_Click;
            // 
            // tasksListBox
            // 
            tasksListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tasksListBox.DrawMode = DrawMode.OwnerDrawFixed;
            tasksListBox.Executer = null;
            tasksListBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            tasksListBox.FormattingEnabled = true;
            tasksListBox.IntegralHeight = false;
            tasksListBox.ItemHeight = 14;
            tasksListBox.Location = new System.Drawing.Point(6, 51);
            tasksListBox.Name = "tasksListBox";
            tasksListBox.SelectedItem = null;
            tasksListBox.SelectionMode = SelectionMode.MultiExtended;
            tasksListBox.Size = new System.Drawing.Size(511, 386);
            tasksListBox.TabIndex = 0;
            tasksListBox.SelectedIndexChanged += tasksListBox_SelectedIndexChanged;
            tasksListBox.DoubleClick += tasksListBox_DoubleClick;
            // 
            // refreshTimer
            // 
            refreshTimer.Enabled = true;
            refreshTimer.Interval = 500;
            refreshTimer.Tick += refreshTimer_Tick;
            // 
            // SynPipelineToolForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(801, 467);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            DoubleBuffered = true;
            Name = "SynPipelineToolForm";
            Text = "Synetic Pipeline Tool";
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private PipelineListBox pipelinesListBox;
        private GroupBox groupBox2;
        private PipelineTaskListBox tasksListBox;
        private Button buttonRemoveP;
        private Button buttonEditP;
        private Button buttonNewP;
        private Button buttonExecuteP;
        private Button buttonRemoveT;
        private Button buttonEditT;
        private Button buttonNewT;
        private Button buttonCopyP;
        private Button buttonDownP;
        private Button buttonUpP;
        private Button buttonDownT;
        private Button buttonUpT;
        private Button buttonCopyT;
        private Timer refreshTimer;
    }
}