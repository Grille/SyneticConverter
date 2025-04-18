﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using SyneticLib.IO.Generic;
using SyneticLib.World;
using OpenTK.Mathematics;
using static SyneticLib.Files.MoxFile;

namespace SyneticLib.IO;
public class ScenarioBeamSerializer : DirectoryFileSerializer<Scenario>
{

    public Vector3 GetPosition(Vector3 pos)
    {
        return new Vector3(pos.X * 0.1f, pos.Z * 0.1f, pos.Y * 0.1f + 150);

    }

    public void SerializeLight(TextWriter writer, Light light)
    {
        const string parent = "Lights";

        var pos = GetPosition(light.Position);

        writer.Write("{");
        writer.Write($"\"__parent\":\"{parent}\",");
        //writer.Write($"\"name\":\"elight_{i++}\",");
        writer.Write($"\"class\":\"SpotLight\",");
        writer.Write($"\"position\":[{pos.X},{pos.Y},{pos.Z}],");
        writer.Write($"\"brightness\":10,");
        writer.Write($"\"color\":[{light.Color.R},{light.Color.G},{light.Color.B}],");
        writer.Write($"\"innerAngle\":0,");
        writer.Write($"\"outerAngle\":170,");
        writer.Write($"\"rotationMatrix\":[1,0,0,0,5.96046448e-08,-0.99999994,0,0.99999994,5.96046448e-08]");
        writer.Write("}");

        writer.WriteLine();

    }

    public void SerializeLights(TextWriter writer, ICollection<Light> lights)
    {
        foreach (var light in lights)
        {
            SerializeLight(writer, light);
        }
    }

    public void SerializeLights(string dirPath, ICollection<Light> lights)
    {
        var filePath = Path.Combine(dirPath, "items.level.json");
        using var writer = new StreamWriter(filePath, false);
        SerializeLights(writer, lights);
    }

    public void SerializePropInstances(string dirPath, ICollection<PropInstance> instances)
    {
        var dict = new Dictionary<string, List<PropInstance>>();

        foreach (var instance in instances)
        {
            var key = instance.Class;
            if (!dict.TryGetValue(key, out var list))
            { 
                list = new List<PropInstance>();
                dict.Add(key, list);
            }
            list.Add(instance);
        }

        foreach (var pair in dict)
        {
            var filePath = Path.Combine(dirPath, $"{pair.Key}.forest4.json");

            using var writer = new StreamWriter(filePath, false);
            SerializePropInstances(writer, pair.Value);
        }
    }

    public void SerializePropInstances(TextWriter writer, ICollection<PropInstance> instances)
    {
        foreach (var instance in instances)
        {
            SerializePropInstance(writer, instance);
        }
    }

    public void SerializePropInstance(TextWriter writer, PropInstance instance)
    {
        var pos = GetPosition(instance.Position);

        writer.Write("{");
        writer.Write($"\"ctxid\":1,");
        writer.Write($"\"pos\":[{pos.X},{pos.Y},{pos.Z}],");
        writer.Write($"\"rotationMatrix\":[1,0,0,0,5.96046448e-08,-0.99999994,0,0.99999994,5.96046448e-08],");
        writer.Write($"\"scale\":1,");
        writer.Write($"\"type\":\"{instance.Class}\"");
        writer.Write("}");

        writer.WriteLine();
    }

    protected override Scenario OnLoad(string dirPath, string fileName)
    {
        throw new NotImplementedException();
    }

    protected override void OnSave(string dirPath, string fileName, Scenario obj)
    {
        var forestPath = Path.Combine(dirPath, "forest");
        Directory.CreateDirectory(forestPath);
        SerializePropInstances(forestPath, obj.PropInstances);

        var lightsPath = Path.Combine(dirPath, "lights");
        Directory.CreateDirectory(lightsPath);
        SerializeLights(lightsPath, obj.Lights);
    }
}
