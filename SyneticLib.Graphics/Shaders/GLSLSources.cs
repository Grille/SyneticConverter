using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace SyneticLib.Graphics.Shaders;
internal static class GLSLSources
{
    public static string TerrainVertex;
    public static string TerrainFragment;
    public static string MeshVertex;
    public static string MeshFragment;
    public static string SpriteVertex;
    public static string SpriteFragment;

    static GLSLSources()
    {
        //TerrainVertex = LoadInternalShaderFile("TerrainVertex");
        //TerrainFragment = LoadInternalShaderFile("TerrainFragment");
        MeshVertex = LoadInternalShaderFile("Mesh.vert");
        MeshFragment = LoadInternalShaderFile("Mesh.frag");
        //SpriteVertex = LoadInternalShaderFile("SpriteVertex");
        //SpriteFragment = LoadInternalShaderFile("SpriteFragment");
    }

    public static string LoadInternalShaderFile(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"SyneticLib.Graphics.Shaders.{name}";
        using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream);
        var result = reader.ReadToEnd();
        return result;
    }

    public static (string Vert, string Frag) LoadInternalShaderFiles(MaterialShaderType type) => type switch
    {
        MaterialShaderType.Default => (MeshVertex, MeshFragment),
        _ => throw new Exception(),
    };
}
