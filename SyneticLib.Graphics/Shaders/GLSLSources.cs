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
    public static GlslVertexShader Mesh;

    public static GlslFragmentShader Terrain;
    public static GlslFragmentShader TerrainWater;
    public static GlslFragmentShader DebugTexture0;

    static GLSLSources()
    {
        Mesh = LoadVertexShaderFile("Mesh");

        Terrain = LoadFragmentShaderFile("Mesh");
        DebugTexture0 = LoadFragmentShaderFile("SimpleColor");
        TerrainWater = LoadFragmentShaderFile("Water");
    }

    public static GlslVertexShader LoadVertexShaderFile(string name) => LoadInternalShaderFile($"vert.{name}.vert");

    public static GlslFragmentShader LoadFragmentShaderFile(string name) => LoadInternalShaderFile($"frag.{name}.frag");

    public static string LoadInternalShaderFile(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"SyneticLib.Graphics.Shaders.{name}";
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null) 
            throw new FileNotFoundException(resourceName);

        using var reader = new StreamReader(stream);
        var result = reader.ReadToEnd();
        return result;
    }

    public static (GlslVertexShader Vert, GlslFragmentShader Frag) LoadInternalShaderFiles(MaterialShaderType type) => type switch
    {
        MaterialShaderType.Default => (Mesh, Terrain),
        MaterialShaderType.Water => (Mesh, TerrainWater),
        MaterialShaderType.Simple => (Mesh, DebugTexture0),
        _ => (Mesh, DebugTexture0),
    };
}

public record GlslVertexShader(string Source)
{
    public static implicit operator GlslVertexShader(string value) => new(value);
    public static implicit operator string(GlslVertexShader value) => value.Source;
}

public record GlslFragmentShader(string Source)
{
    public static implicit operator GlslFragmentShader(string value) => new(value);
    public static implicit operator string(GlslFragmentShader value) => value.Source;
}