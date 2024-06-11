using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticPipelineTool.GUI;
internal static class ExceptionBox
{
    public static DialogResult Show(IWin32Window owner, Action action, MessageBoxButtons buttons = MessageBoxButtons.OK)
    {
        try
        {
            action();
            return DialogResult.Continue;
        }
        catch (Exception e)
        {
            return Show(owner, e, buttons);
        }
    }

    public static DialogResult Show(IWin32Window owner, Exception e, MessageBoxButtons buttons = MessageBoxButtons.OK)
    {
        return MessageBox.Show(owner, e.Message, e.GetType().Name, buttons, MessageBoxIcon.Error);
    }
}
