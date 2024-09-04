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
        var bmp = new TextureBitmapSerializer(ImageFormat.Bmp);
        var png = new TextureBitmapSerializer(ImageFormat.Png);
        var jpg = new TextureBitmapSerializer(ImageFormat.Jpeg);

        Serializers.Texture.Registry.Add("bmp", bmp);
        Serializers.Texture.Registry.Add("png", png);
        Serializers.Texture.Registry.Add("jpg", jpg);
        Serializers.Texture.Registry.Add("jpeg", jpg);
    }

    public SyneticPipelineToolForm()
    {
        Icon = EmbeddedImageList.SyneticLib.Icon;
    }

    private void InitializeComponent()
    {

    }
}
