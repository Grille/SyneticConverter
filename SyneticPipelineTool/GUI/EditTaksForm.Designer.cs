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
            textBox = new System.Windows.Forms.TextBox();
            comboBoxType = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            groupBoxParameters = new System.Windows.Forms.GroupBox();
            panelParameters = new System.Windows.Forms.Panel();
            buttonCancel = new System.Windows.Forms.Button();
            buttonOK = new System.Windows.Forms.Button();
            button1 = new System.Windows.Forms.Button();
            groupBoxParameters.SuspendLayout();
            SuspendLayout();
            // 
            // textBox
            // 
            textBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBox.Location = new System.Drawing.Point(12, 41);
            textBox.Name = "textBox";
            textBox.ReadOnly = true;
            textBox.Size = new System.Drawing.Size(1054, 23);
            textBox.TabIndex = 0;
            textBox.TextChanged += textBox_TextChanged;
            // 
            // comboBoxType
            // 
            comboBoxType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            comboBoxType.FormattingEnabled = true;
            comboBoxType.Location = new System.Drawing.Point(85, 12);
            comboBoxType.Name = "comboBoxType";
            comboBoxType.Size = new System.Drawing.Size(929, 23);
            comboBoxType.TabIndex = 1;
            comboBoxType.SelectedIndexChanged += comboBoxType_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 15);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(56, 15);
            label1.TabIndex = 2;
            label1.Text = "Task Type";
            // 
            // groupBoxParameters
            // 
            groupBoxParameters.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBoxParameters.Controls.Add(panelParameters);
            groupBoxParameters.Location = new System.Drawing.Point(12, 70);
            groupBoxParameters.Name = "groupBoxParameters";
            groupBoxParameters.Size = new System.Drawing.Size(1054, 464);
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
            panelParameters.Size = new System.Drawing.Size(1048, 442);
            panelParameters.TabIndex = 0;
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonCancel.Location = new System.Drawing.Point(991, 540);
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
            buttonOK.Location = new System.Drawing.Point(910, 540);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new System.Drawing.Size(75, 23);
            buttonOK.TabIndex = 5;
            buttonOK.Text = "OK";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Click += buttonOK_Click;
            // 
            // button1
            // 
            button1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            button1.Location = new System.Drawing.Point(1020, 12);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(46, 23);
            button1.TabIndex = 6;
            button1.Text = "...";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // EditTaksForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1078, 575);
            Controls.Add(button1);
            Controls.Add(buttonOK);
            Controls.Add(buttonCancel);
            Controls.Add(groupBoxParameters);
            Controls.Add(label1);
            Controls.Add(comboBoxType);
            Controls.Add(textBox);
            Name = "EditTaksForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "EditTaksForm";
            groupBoxParameters.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxParameters;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Panel panelParameters;
        private System.Windows.Forms.Button button1;
    }
}