using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SyneticLib;
using static SyneticLib.IO.Synetic.Files.LvlFile;

namespace SyneticTool.Nodes;

public class DataTreeNode : TreeNode
{
    public virtual bool Loaded { get; private set; }

    public Ressource DataValue { get; set; }

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
        if (DataValue.NeedSeek)
        {
            DataValue.Seek();
        }

        if (!Loaded)
        {
            OnUpdateContent();
            Loaded = true;
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
}
