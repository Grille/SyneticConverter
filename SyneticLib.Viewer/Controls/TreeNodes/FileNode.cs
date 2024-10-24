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

    public FileNode(string filePath)
    {
        FilePath = filePath;
        Text = FileName;

        Update();
    }

    public void Update()
    {
        switch (FileExtension.ToLower())
        {
            case ".dds":
            case ".tga":
            case ".ptx":
            {
                Icon = EmbeddedImageList.Texture.Bitmap16;
                break;
            }
            case ".mox":
            {
                Icon = EmbeddedImageList.Mesh.Bitmap16;
                break;
            }
            case ".mtl":
            {
                Icon = EmbeddedImageList.Misc.Bitmap16;
                break;
            }
            case ".wav":
            {
                Icon = EmbeddedImageList.Audio.Bitmap16;
                break;
            }
            case ".lvl":
            case ".sni":
            case ".idx":
            case ".vtx":
            case ".qad":
            case ".geo":
            {
                Icon = EmbeddedImageList.Terrain.Bitmap16;
                break;
            }
            default:
            {
                Icon = EmbeddedImageList.File.Bitmap16;
                break;
            };
        }
    }
}