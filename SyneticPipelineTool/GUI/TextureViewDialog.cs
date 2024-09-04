using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SyneticLib;

using SyneticPipelineTool.Tasks.IO;

namespace SyneticPipelineTool.GUI;

public partial class TextureViewDialog : Form
{
    public TextureViewDialog()
    {
        InitializeComponent();
    }

    public static DialogResult ShowDialog(Texture texture)
    {
        using var dialog = new TextureViewDialog();
        dialog.GdiTextureView.SubmitTexture(texture);
        return dialog.ShowDialog();
    }
}
