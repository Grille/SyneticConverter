using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SyneticLib.Files.LvlFile;
using static SyneticLib.Files.SniFile;

namespace SyneticTool.Nodes;

public class BaseNode : TreeNode
{
    public MainForm MainForm => (MainForm)TreeView.FindForm();

    public object Value { get; }

    public BaseNode(object obj)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        Value = obj;
    }

    public int Image
    {
        get => ImageIndex;
        set
        {
            SelectedImageIndex = ImageIndex = value;
        }
    }

    public void UpdateAppearance()
    {
        OnUpdateAppearance();
    }

    protected virtual void OnUpdateContent()
    {

    }

    protected virtual void OnUpdateAppearance()
    {

    }

    public virtual void OnShown()
    {
        OnUpdateContent();
        UpdateAppearance();
    }

    public virtual void OnSelect(TreeViewCancelEventArgs e)
    {

    }

    public virtual void OnExpand(TreeViewCancelEventArgs e)
    {

    }
}
