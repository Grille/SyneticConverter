using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticPipelineTool.GUI;

internal class TypeTreeNode : TreeNode
{
    public AssemblyTaskTypeTree.TypeInfo Object { get; }
    public TypeTreeNode(AssemblyTaskTypeTree.TreeNode entry)
    {

        Object = entry.Value;
        //Text = Object.Name;
    }
}
