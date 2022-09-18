namespace SyneticBasicTools
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.buttonBSrc = new System.Windows.Forms.Button();
            this.buttonConvert = new System.Windows.Forms.Button();
            this.buttonBDst = new System.Windows.Forms.Button();
            this.comboBoxVDst = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxVSrc = new System.Windows.Forms.ComboBox();
            this.textBoxDDst = new System.Windows.Forms.TextBox();
            this.textBoxDSrc = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(739, 214);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.buttonBSrc);
            this.tabPage1.Controls.Add(this.buttonConvert);
            this.tabPage1.Controls.Add(this.buttonBDst);
            this.tabPage1.Controls.Add(this.comboBoxVDst);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.comboBoxVSrc);
            this.tabPage1.Controls.Add(this.textBoxDDst);
            this.tabPage1.Controls.Add(this.textBoxDSrc);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(731, 186);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "VtxIdx/Geo Converter";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // buttonBSrc
            // 
            this.buttonBSrc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBSrc.Location = new System.Drawing.Point(648, 6);
            this.buttonBSrc.Name = "buttonBSrc";
            this.buttonBSrc.Size = new System.Drawing.Size(75, 23);
            this.buttonBSrc.TabIndex = 11;
            this.buttonBSrc.Text = "Browse...";
            this.buttonBSrc.UseVisualStyleBackColor = true;
            this.buttonBSrc.Click += new System.EventHandler(this.buttonBSrc_Click);
            // 
            // buttonConvert
            // 
            this.buttonConvert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConvert.Location = new System.Drawing.Point(8, 156);
            this.buttonConvert.Name = "buttonConvert";
            this.buttonConvert.Size = new System.Drawing.Size(715, 23);
            this.buttonConvert.TabIndex = 10;
            this.buttonConvert.Text = "Convert";
            this.buttonConvert.UseVisualStyleBackColor = true;
            this.buttonConvert.Click += new System.EventHandler(this.buttonConvert_Click);
            // 
            // buttonBDst
            // 
            this.buttonBDst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBDst.Location = new System.Drawing.Point(648, 87);
            this.buttonBDst.Name = "buttonBDst";
            this.buttonBDst.Size = new System.Drawing.Size(75, 23);
            this.buttonBDst.TabIndex = 9;
            this.buttonBDst.Text = "Browse...";
            this.buttonBDst.UseVisualStyleBackColor = true;
            this.buttonBDst.Click += new System.EventHandler(this.buttonBDst_Click);
            // 
            // comboBoxVDst
            // 
            this.comboBoxVDst.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxVDst.FormattingEnabled = true;
            this.comboBoxVDst.Location = new System.Drawing.Point(136, 116);
            this.comboBoxVDst.Name = "comboBoxVDst";
            this.comboBoxVDst.Size = new System.Drawing.Size(587, 23);
            this.comboBoxVDst.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Input Version:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Output Directory:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Input Directory:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Output Version:";
            // 
            // comboBoxVSrc
            // 
            this.comboBoxVSrc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxVSrc.FormattingEnabled = true;
            this.comboBoxVSrc.Location = new System.Drawing.Point(136, 35);
            this.comboBoxVSrc.Name = "comboBoxVSrc";
            this.comboBoxVSrc.Size = new System.Drawing.Size(587, 23);
            this.comboBoxVSrc.TabIndex = 2;
            // 
            // textBoxDDst
            // 
            this.textBoxDDst.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDDst.Location = new System.Drawing.Point(136, 87);
            this.textBoxDDst.Name = "textBoxDDst";
            this.textBoxDDst.Size = new System.Drawing.Size(506, 23);
            this.textBoxDDst.TabIndex = 1;
            this.textBoxDDst.TextChanged += new System.EventHandler(this.textBoxDDst_TextChanged);
            // 
            // textBoxDSrc
            // 
            this.textBoxDSrc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDSrc.Location = new System.Drawing.Point(136, 6);
            this.textBoxDSrc.Name = "textBoxDSrc";
            this.textBoxDSrc.Size = new System.Drawing.Size(506, 23);
            this.textBoxDSrc.TabIndex = 0;
            this.textBoxDSrc.TextChanged += new System.EventHandler(this.textBoxDSrc_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 214);
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "SyneticTools";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private Button buttonBSrc;
        private Button buttonConvert;
        private Button buttonBDst;
        private ComboBox comboBoxVDst;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private ComboBox comboBoxVSrc;
        private TextBox textBoxDDst;
        private TextBox textBoxDSrc;
    }
}