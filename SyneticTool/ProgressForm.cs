using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SyneticLib;

namespace SyneticTool;

public partial class ProgressForm : Form
{
    Ressource target;
    public ProgressForm(Ressource target)
    {
        InitializeComponent();
        this.target = target;

        //target.ProgressInfo.HasUpdated += ProgressInfo_HasUpdated;
        Update(0, "Loading...");
    }

    private void ProgressInfo_HasUpdated(object sender, EventArgs e)
    {
        var info = (ProgressLogger)sender;
        Update(info.Value, info.Description);
    }

    public void Update(float value, string text)
    {
        ProgressBar.Value = (int)value * 100;
        MSGLabel.Text = text;
        Refresh();
    }
}
