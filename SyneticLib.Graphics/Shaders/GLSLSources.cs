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
    public readonly static GlslVertexShaderSource VSprite;
    public readonly static GlslVertexShaderSource VMesh;
    public readonly static GlslVertexShaderSource VModel;
    public readonly static GlslVertexShaderSource VTerrain;
    public readonly static GlslVertexShaderSource VFrame;

    public readonly static GlslFragmentShaderSource Mesh;
    public readonly static GlslFragmentShaderSource Model;
    public readonly static GlslFragmentShaderSource Terrain;
    public readonly static GlslFragmentShaderSource TerrainWater;
    public readonly static GlslFragmentShaderSource DebugTexture0;
    public readonly static GlslFragmentShaderSource Sprite;
    public readonly static GlslFragmentShaderSource Text;
    public readonly static GlslFragmentShaderSource Frame;

    static GLSLSources()
    {
        VSprite = LoadVertexShaderFile("Sprite");
        VMesh = LoadVertexShaderFile("Mesh");
        VTerrain = LoadVertexShaderFile("Terrain");
        VModel = LoadVertexShaderFile("Model");
        VFrame = LoadVertexShaderFile("Frame");

        Terrain = LoadFragmentShaderFile("Terrain");
        Model = LoadFragmentShaderFile("Model");
        Mesh = LoadFragmentShaderFile("Mesh");
        DebugTexture0 = LoadFragmentShaderFile("SimpleColor");
        TerrainWater = LoadFragmentShaderFile("Water");
        Sprite = LoadFragmentShaderFile("Sprite");
        Text = LoadFragmentShaderFile("Text");
        Frame = LoadFragmentShaderFile("Frame");
    }

    public static GlslVertexShaderSource LoadVertexShaderFile(string name) => LoadInternalShaderFile($"vert.{name}.vert");

    public static GlslFragmentShaderSource LoadFragmentShaderFile(string name) => LoadInternalShaderFile($"frag.{name}.frag");

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

    /*
    public static (GlslVertexShaderSource Vert, GlslFragmentShaderSource Frag) LoadInternalShaderFiles(MaterialShaderType type) => type switch
    {
        MaterialShaderType.Default => (VMesh, Terrain),
        MaterialShaderType.Water => (VMesh, TerrainWater),
        MaterialShaderType.Simple => (VMesh, DebugTexture0),
        _ => (VMesh, DebugTexture0),
    };
    */
}

public record struct GlslVertexShaderSource(string Source)
{
    public static implicit operator GlslVertexShaderSource(string value) => new(value);
    public static implicit operator string(GlslVertexShaderSource value) => value.Source;
}

public record struct GlslFragmentShaderSource(string Source)
{
    public static implicit operator GlslFragmentShaderSource(string value) => new(value);
    public static implicit operator string(GlslFragmentShaderSource value) => value.Source;
}