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

static class SerializerTaskUtils
{
    public readonly static string Auto = "Auto";
    public readonly static string Default = Auto;
    public readonly static string[] TextureFileTypes = new string[] { Auto, ".BMP", ".DDS", ".PNG", ".PTX", ".TGA", ".JPG" };
    public readonly static string[] MeshFileTypes = new string[] { Auto, ".COB", ".CPO", ".MOX", ".OBJ" };
    public readonly static string[] ModelTypes = new string[] { "Synetic (Mox/Mtl/Ptx)", "Wavefront (Obj/Mtl/Dds)" };

    public static string GetFilter()
    {
        var sb = new StringBuilder();
        sb.Append("Supported Files|");
        foreach (var type in TextureFileTypes)
        {
            sb.Append(" *");
            sb.Append(type.ToLower());
        }
        return sb.ToString();
    }

    private static string? GetKey(string key)
    {
        return key == Auto ? null : key;
    }

    public static void Save<T>(SerializerRegistry<T> registry, string path, string type, VariableValue value)
    {
        registry.Save(path, (T)value.Value, GetKey(type));
    }

    public static void SaveTexture(string path, string type, VariableValue value)
    {
        Save(Serializers.Texture.Registry, path, type, value);
    }

    public static void SaveMesh(string path, string type, VariableValue value)
    {
        Save(Serializers.Mesh.Registry, path, type, value);
    }

    public static void SaveModel(string path, string type, VariableValue value)
    {
        var key = type.Split(' ', 2)[0].ToLowerInvariant();
        Save(Serializers.Model.Registry, path, key, value);
    }

    public static VariableValue Load<T>(SerializerRegistry<T> registry, string path, string type)
    {
        var texture = registry.Load(path, GetKey(type));
        return new VariableValue(texture);
    }

    public static VariableValue LoadTexture(string path, string type)
    {
        return Load(Serializers.Texture.Registry, path, type);
    }

    public static VariableValue LoadMesh(string path, string type)
    {
        return Load(Serializers.Mesh.Registry, path, type);
    }

    public static VariableValue LoadModel(string path, string type)
    {
        var key = type.Split(' ', 2)[0].ToLowerInvariant();
        return Load(Serializers.Model.Registry, path, key);
    }
}
