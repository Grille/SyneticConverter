using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;

using Grille.PipelineTool;

using SyneticLib.IO;
using SyneticLib.WinForms.Resources;
using SyneticLib.WinForms.IO;

namespace SyneticPipelineTool.GUI;

public class SyneticPipelineToolForm : PipelineToolForm
{
    static SyneticPipelineToolForm()
    {
        Serializers.Texture.Registry.Add("png", new TextureBitmapSerializer(ImageFormat.Png));
        Serializers.Texture.Registry.Add("bmp", new TextureBitmapSerializer(ImageFormat.Bmp));
    }

    public SyneticPipelineToolForm()
    {
        Icon = EmbeddedImageList.SyneticLib.Icon;
    }

    private void InitializeComponent()
    {

    }
}
