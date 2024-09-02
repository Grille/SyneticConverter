using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.PipelineTool;

using SyneticLib;
using SyneticLib.IO;


namespace SyneticPipelineTool.Tasks.IO;

static class TextureTaskUtils
{
    public readonly static string Auto = "Auto";
    public readonly static string Default = Auto;
    public readonly static string[] FileTypes = new string[] { Auto, ".BMP", ".DDS", ".PNG", ".PTX", ".TGA" };

    public static void Save(string path, string type, VariableValue value)
    {
        if (type == Auto)
        {
            Serializers.Texture.Registry.Save(path, (Texture)value.Value);
        }
        else
        {
            Serializers.Texture.Registry.Save(path, (Texture)value.Value, type);
        }
    }

    public static VariableValue Load(string path, string type)
    {
        if (type == Auto)
        {
            var texture = Serializers.Texture.Registry.Load(path);
            return new VariableValue(texture);
        }
        else
        {
            var texture = Serializers.Texture.Registry.Load(path, type);
            return new VariableValue(texture);
        }
    }
}
