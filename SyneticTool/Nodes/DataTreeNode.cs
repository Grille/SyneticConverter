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
            Console.WriteLine("seek");
            DataValue.Seek();
            OnUpdateContent();
        }
        OnUpdateAppearance();
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
}
