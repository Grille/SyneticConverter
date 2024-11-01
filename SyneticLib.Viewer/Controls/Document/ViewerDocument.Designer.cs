namespace SyneticLib.WinForms.Controls
{
    partial class ViewerDocument
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
            ViewerControl = new OpenGL.ViewerControl();
            renderTimer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // ViewerControl
            // 
            ViewerControl.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            ViewerControl.APIVersion = new System.Version(3, 3, 0, 0);
            ViewerControl.BackColor = System.Drawing.Color.Black;
            ViewerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            ViewerControl.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            ViewerControl.IsEventDriven = true;
            ViewerControl.Location = new System.Drawing.Point(0, 0);
            ViewerControl.Name = "ViewerControl";
            ViewerControl.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
            ViewerControl.Size = new System.Drawing.Size(506, 521);
            ViewerControl.TabIndex = 0;
            ViewerControl.Text = "viewerControl1";
            ViewerControl.Click += viewerControl1_Click;
            // 
            // renderTimer
            // 
            renderTimer.Interval = 10;
            renderTimer.Tick += renderTimer_Tick;
            // 
            // ViewerDocument
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(ViewerControl);
            DockText = "Viewer";
            Name = "ViewerDocument";
            Size = new System.Drawing.Size(506, 521);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Timer renderTimer;
        public OpenGL.ViewerControl ViewerControl;
    }
}
