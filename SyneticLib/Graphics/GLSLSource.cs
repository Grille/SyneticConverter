using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace SyneticLib.Graphics;
internal static class GLSLSource
{
    public static string TerrainVertex;
    public static string TerrainFragment;
    public static string MeshVertex;
    public static string MeshFragment;
    public static string SpriteVertex;
    public static string SpriteFragment;

    static GLSLSource()
    {
        TerrainVertex = LoadInternalShader("TerrainVertex");
        TerrainFragment = LoadInternalShader("TerrainFragment");
        MeshVertex = LoadInternalShader("MeshVertex");
        MeshFragment = LoadInternalShader("MeshFragment");
        SpriteVertex = LoadInternalShader("SpriteVertex");
        SpriteFragment = LoadInternalShader("SpriteFragment");
    }

    public static string LoadInternalShader(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"SyneticLib.Graphics.Shaders.{name}.glsl";
        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        using (StreamReader reader = new StreamReader(stream))
        {
            string result = reader.ReadToEnd();
            return result;
        }
    }




}
