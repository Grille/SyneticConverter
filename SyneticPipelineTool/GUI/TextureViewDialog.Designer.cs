namespace SyneticPipelineTool.GUI
{
    partial class TextureViewDialog
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
            SyneticLib.WinForms.Drawing.GdiCamera gdiCamera1 = new SyneticLib.WinForms.Drawing.GdiCamera();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextureViewDialog));
            GdiTextureView = new SyneticLib.WinForms.Controls.GdiTextureView();
            SuspendLayout();
            // 
            // GdiTextureView
            // 
            GdiTextureView.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            gdiCamera1.ScreenSize = (OpenTK.Mathematics.Vector2)resources.GetObject("gdiCamera1.ScreenSize");
            GdiTextureView.Camera = gdiCamera1;
            GdiTextureView.Dock = System.Windows.Forms.DockStyle.Fill;
            GdiTextureView.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            GdiTextureView.ForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
            GdiTextureView.Location = new System.Drawing.Point(0, 24);
            GdiTextureView.Name = "GdiTextureView";
            GdiTextureView.Size = new System.Drawing.Size(800, 426);
            GdiTextureView.TabIndex = 0;
            GdiTextureView.Text = "gdiTextureView1";

            // 
            // TextureViewDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(GdiTextureView);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SyneticLib.WinForms.Controls.GdiTextureView GdiTextureView;
    }
}