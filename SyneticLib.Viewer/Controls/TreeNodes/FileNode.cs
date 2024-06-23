using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DarkUI.Controls;

using SyneticLib.WinForms.Resources;

namespace SyneticLib.WinForms.Controls.TreeNodes;

public class FileNode : DarkTreeNode
{
    public string FilePath { get; }

    public string FileName => Path.GetFileName(FilePath);

    public string FileExtension => Path.GetExtension(FilePath);

    public FileNode(string path)
    {
        FilePath = path;
        Text = FileName;

        switch (FileExtension.ToLower())
        {
            case ".dds":
            case ".tga":
            case ".ptx":
            {
                Icon = EmbeddedImageList.Texture.Bitmap16;
                break;
            }
        }
    }
}
