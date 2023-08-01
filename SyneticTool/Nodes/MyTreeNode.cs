using SyneticLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SyneticLib.LowLevel.Files.LvlFile;

namespace SyneticTool.Nodes;

public class MyTreeNode : TreeNode
{
    public MainForm MainForm => (MainForm)TreeView.FindForm();

    public object Object { get; }

    public MyTreeNode(object obj)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        Object = obj;
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
