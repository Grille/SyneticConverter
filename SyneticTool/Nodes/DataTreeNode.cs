using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SyneticLib;

namespace SyneticTool.Nodes;

public class DataTreeNode : TreeNode
{
    public bool Loaded { get; private set; } = false;

    public Ressource DataValue { get; set; }

    public MainForm MainForm => (MainForm)TreeView.FindForm();

    public DataTreeNode(Ressource data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        DataValue = data;
    }

    public int Image
    {
        get => ImageIndex;
        set
        {
            SelectedImageIndex = ImageIndex = value;
        }
    }

    public void UpdateAppearance() {
        OnUpdateAppearance();
    }

    protected virtual void OnUpdateContent()
    {

    }

    protected virtual void OnUpdateAppearance()
    {
        Name = DataValue.FileName;
        Text = Name;

        ForeColor = NodeColors.RessourceColor(DataValue);
    }

    public virtual void OnShown()
    {
        if (DataValue.PointerState == PointerState.Exists && DataValue.NeedSeek)
        {
            DataValue.Seek();
        }

        if (!Loaded)
        {
            OnUpdateContent();
            UpdateAppearance();

            Loaded = true;
        }
    }

    public virtual void OnSelect(TreeViewCancelEventArgs e)
    {

    }

    public virtual void OnExpand(TreeViewCancelEventArgs e)
    {

    }
}
