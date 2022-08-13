using SyneticLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticTool.Nodes;

public class SoundNode : DataTreeNode
{
    public SoundNode(Sound data) : base(data)
    {
        SelectedImageIndex = ImageIndex = IconList.Audio;
    }
}
