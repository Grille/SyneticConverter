namespace SyneticTool
{
    partial class ScenarioInspector
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
            this.components = new System.ComponentModel.Container();
            this.renderTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // renderTimer
            // 
            this.renderTimer.Tick += new System.EventHandler(this.renderTimer_Tick);
            // 
            // ScenarioInspector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "ScenarioInspector";
            this.Text = "ScenarioInspector";
            this.Shown += new System.EventHandler(this.ScenarioInspector_Shown);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ScenarioInspector_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ScenarioInspector_MouseMove);
            this.Resize += new System.EventHandler(this.ScenarioInspector_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer renderTimer;
    }
}