using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SyneticLib.Graphics;

namespace SyneticLib.Viewer;

public partial class ExportSceneAsObjDialog : Form
{
    public ExportSceneAsObjDialog()
    {
        InitializeComponent();
    }

    public void ShowDialog(IWin32Window owner, GlScene scene)
    {
        ShowDialog(this);
    }
}
