using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticLib.WinForms;

public static class ExceptionBox
{
    public static DialogResult Show(IWin32Window owner, Exception exception)
    {
        return MessageBox.Show(owner, exception.ToString(), exception.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public static DialogResult Show(IWin32Window owner, string text, string caption)
    {
        return MessageBox.Show(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
