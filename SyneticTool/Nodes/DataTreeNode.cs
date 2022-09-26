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
    public virtual bool Loaded { get; private set; }

    public Ressource DataValue { get; set; }

    public MainForm MainForm => (MainForm)TreeView.FindForm();

    public DataTreeNode(Ressource data)
    {
        if (data == null)
            throw new ArgumentNullException("data");

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

    public void SeekAndUpdateContent()
    {
        DataValue.UpdatePointer();

        if (DataValue.PointerState == PointerState.Exists)
        {

            //if (DataValue.NeedSeek)
            //{
                DataValue.Seek();
            //}

            OnUpdateContent();
        }

        UpdateAppearance();
    }

    public void UpdateAppearance() {
        //if (!IsVisible)
        //    return;

        OnUpdateAppearance();

        if (IsExpanded)
        {
            foreach (var node in Nodes)
            {
                if (node is DataTreeNode)
                {
                    ((DataTreeNode)node).UpdateAppearance();
                }
            }
        }
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
        DataValue.UpdatePointer();
        if (DataValue.NeedSeek && DataValue.PointerState == PointerState.Exists)
            DataValue.Seek();
        UpdateAppearance();
    }

    public virtual void OnSelect(TreeViewCancelEventArgs e)
    {

    }

    public virtual void OnExpand(TreeViewCancelEventArgs e)
    {

    }
}
