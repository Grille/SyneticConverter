namespace SyneticLib.WinForms
{
    partial class ProgressDialog
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
            progressBar1 = new System.Windows.Forms.ProgressBar();
            label1 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // progressBar1
            // 
            progressBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            progressBar1.Location = new System.Drawing.Point(12, 12);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(539, 23);
            progressBar1.TabIndex = 0;
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label1.Location = new System.Drawing.Point(12, 38);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(539, 23);
            label1.TabIndex = 1;
            label1.Text = "label1";
            // 
            // ProgressDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(563, 66);
            ControlBox = false;
            Controls.Add(label1);
            Controls.Add(progressBar1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ProgressDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "ProgressDialog";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
    }
}