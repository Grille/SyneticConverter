namespace SyneticPipelineTool
{
    partial class EditTaksForm
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node2");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] { treeNode1 });
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Node1");
            textBox = new System.Windows.Forms.TextBox();
            groupBoxParameters = new System.Windows.Forms.GroupBox();
            panelParameters = new System.Windows.Forms.Panel();
            buttonCancel = new System.Windows.Forms.Button();
            buttonOK = new System.Windows.Forms.Button();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            treeViewTypes = new System.Windows.Forms.TreeView();
            buttonReload = new System.Windows.Forms.Button();
            textBoxType = new System.Windows.Forms.TextBox();
            groupBoxParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // textBox
            // 
            textBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBox.Location = new System.Drawing.Point(6, 32);
            textBox.Name = "textBox";
            textBox.ReadOnly = true;
            textBox.Size = new System.Drawing.Size(813, 23);
            textBox.TabIndex = 0;
            // 
            // groupBoxParameters
            // 
            groupBoxParameters.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBoxParameters.Controls.Add(panelParameters);
            groupBoxParameters.Location = new System.Drawing.Point(3, 61);
            groupBoxParameters.Name = "groupBoxParameters";
            groupBoxParameters.Size = new System.Drawing.Size(816, 580);
            groupBoxParameters.TabIndex = 3;
            groupBoxParameters.TabStop = false;
            groupBoxParameters.Text = "Parameters";
            // 
            // panelParameters
            // 
            panelParameters.AutoScroll = true;
            panelParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            panelParameters.Location = new System.Drawing.Point(3, 19);
            panelParameters.Name = "panelParameters";
            panelParameters.Size = new System.Drawing.Size(810, 558);
            panelParameters.TabIndex = 0;
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonCancel.Location = new System.Drawing.Point(744, 647);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new System.Drawing.Size(75, 23);
            buttonCancel.TabIndex = 4;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonOK
            // 
            buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonOK.Location = new System.Drawing.Point(663, 647);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new System.Drawing.Size(75, 23);
            buttonOK.TabIndex = 5;
            buttonOK.Text = "OK";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Click += buttonOK_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(treeViewTypes);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(textBoxType);
            splitContainer1.Panel2.Controls.Add(buttonReload);
            splitContainer1.Panel2.Controls.Add(groupBoxParameters);
            splitContainer1.Panel2.Controls.Add(buttonCancel);
            splitContainer1.Panel2.Controls.Add(buttonOK);
            splitContainer1.Panel2.Controls.Add(textBox);
            splitContainer1.Size = new System.Drawing.Size(1239, 673);
            splitContainer1.SplitterDistance = 413;
            splitContainer1.TabIndex = 7;
            // 
            // treeViewTypes
            // 
            treeViewTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            treeViewTypes.HideSelection = false;
            treeViewTypes.Location = new System.Drawing.Point(0, 0);
            treeViewTypes.Name = "treeViewTypes";
            treeNode1.Name = "Node2";
            treeNode1.Text = "Node2";
            treeNode2.Name = "Node0";
            treeNode2.Text = "Node0";
            treeNode3.Name = "Node1";
            treeNode3.Text = "Node1";
            treeViewTypes.Nodes.AddRange(new System.Windows.Forms.TreeNode[] { treeNode2, treeNode3 });
            treeViewTypes.Size = new System.Drawing.Size(413, 673);
            treeViewTypes.TabIndex = 0;
            treeViewTypes.BeforeSelect += treeViewTypes_BeforeSelect;
            treeViewTypes.AfterSelect += treeViewTypes_AfterSelect;
            // 
            // buttonReload
            // 
            buttonReload.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonReload.Location = new System.Drawing.Point(582, 647);
            buttonReload.Name = "buttonReload";
            buttonReload.Size = new System.Drawing.Size(75, 23);
            buttonReload.TabIndex = 7;
            buttonReload.Text = "Reload";
            buttonReload.UseVisualStyleBackColor = true;
            buttonReload.Click += buttonReload_Click;
            // 
            // textBoxType
            // 
            textBoxType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBoxType.Location = new System.Drawing.Point(6, 3);
            textBoxType.Name = "textBoxType";
            textBoxType.ReadOnly = true;
            textBoxType.Size = new System.Drawing.Size(813, 23);
            textBoxType.TabIndex = 8;
            // 
            // EditTaksForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1239, 673);
            Controls.Add(splitContainer1);
            DoubleBuffered = true;
            Name = "EditTaksForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "EditTaksForm";
            groupBoxParameters.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.GroupBox groupBoxParameters;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Panel panelParameters;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeViewTypes;
        private System.Windows.Forms.Button buttonReload;
        private System.Windows.Forms.TextBox textBoxType;
    }
}