using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticTool;

public partial class ProgressForm : Form
{
    public ProgressForm()
    {
        InitializeComponent();
        Update(0, "Loading");
    }

    public void Update(int value, string text)
    {
        ProgressBar.Value = value;
        MSGLabel.Text = text;
        Refresh();
    }
}
