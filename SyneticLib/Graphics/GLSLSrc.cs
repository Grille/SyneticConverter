using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace SyneticLib.Graphics;
internal static class GLSLSrc
{
    public static string TerrainVertexSrc;
    public static string MeshVertexSrc;
    public static string FragmentSrc;

    static GLSLSrc()
    {
        TerrainVertexSrc = LoadInternalShader("TerrainVertex");
        MeshVertexSrc = LoadInternalShader("MeshVertex");
        FragmentSrc = LoadInternalShader("Fragment");
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
