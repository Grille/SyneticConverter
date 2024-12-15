using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DarkUI.Docking;

namespace SyneticLib.WinForms.Controls
{
    public partial class ViewerDocument : DarkDocument
    {
        public ViewerDocument()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            renderTimer.Start();
        }

        private void viewerControl1_Click(object sender, EventArgs e)
        {

        }

        private void renderTimer_Tick(object sender, EventArgs e)
        {
            if (!Visible)
                return;

            ViewerControl.UpdateLogic();
            ViewerControl.Invalidate();
        }
    }
}
