using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;
using SyneticLib.Locations;
using SyneticLib.World;
using SyneticLib.IO.Generic;
using OpenTK.Mathematics;

namespace SyneticLib.IO;
public class ScenarioSyneticSerializer : DirectoryFileSerializer<Scenario>
{
    protected override void OnSave(string dirPath, string fileName, Scenario obj)
    {
        throw new NotImplementedException();
    }

    protected override Scenario OnLoad(string dirPath, string fileName)
    {
        var files = new ScenarioFiles();

        files.Load(dirPath, fileName);

        var syn = files.Syn;
        var lvl = files.Lvl;
        var sni = files.Sni;
        var qad = files.Qad;
        var sky = files.Sky;

        var terrainTextures = new TextureDirectory(Path.Combine(dirPath, "textures"));
        var modelTextures = new TextureDirectory(Path.Combine(dirPath, "objects/textures"));
        var models = new ModelDirectory(Path.Combine(dirPath, "objects"), modelTextures);

        var scenario = new Scenario(0);

        scenario.Width = qad.Head.BlockCountX;
        scenario.Height = qad.Head.BlockCountZ;

        scenario.Chunks = new ScenarioChunk[qad.Head.BlockCountX, qad.Head.BlockCountZ];

        scenario.Terrain = Serializers.Terrain.Synetic.Load(files, terrainTextures);

        var lights = new Light[qad.Lights.Length];
        for (int i = 0; i <lights.Length; i++)
        {
            var src = qad.Lights[i];
            var light = new Light(src.Matrix.ExtractTranslation(), src.Color);
            lights[i] = light;
        }

        scenario.Lights = lights;

        var instances = new PropInstance[qad.PropInstances.Length];

        var names = new string[qad.PropClassObjNames.Length];
        for (int i = 0; i < names.Length; i++)
        {
            names[i] = qad.PropClassObjNames[i];
        }

        for (int i = 0; i < instances.Length; i++)
        {
            var src = qad.PropInstances[i];
            var instance = new PropInstance(names[src.ClassId]);
            instance.Position = src.Position;
            instance.Matrix = Matrix3.Identity;
            instances[i] = instance;
        }

        scenario.PropInstances = instances;
        
        // Props
        /*
        var indexedModels = models.CreateIndexedArray(qad.PropClassObjNames);
        var propClasses = new Dictionary<string, PropClass>();
        for (var i = 0; i < qad.Head.PropClassCount; i++)
        {
            var name = qad.PropClassObjNames[i];
            var info = qad.PropClassInfo[i];
            var prop = new PropClass()
            {
                Name = name,
                AnimationMode = info.Mode,
                ColliShape = info.Shape,
                Model = indexedModels[i],
            };
            propClasses[name] = prop;
        }

        var propInstances = new PropInstance[qad.PropInstances.Length];
        for (var i = 0; i < qad.Head.PropInstanceCount; i++)
        {
            var info = qad.PropInstances[i];
            var prop = propClasses[info.Name];
            var Instances = new PropInstance(prop)
            {
                Class = prop,
            };
        }
        */
        // Chunks
        for (int i = 0; i < qad.Chunks.Length; i++)
        {
            var src = qad.Chunks[i];
            var cinfo = new ScenarioChunkCreateInfo(src)
            {
                Terrain = scenario.Terrain,
            };

            var chunk = new ScenarioChunk(cinfo);
            scenario.Chunks[cinfo.Position.X, cinfo.Position.Z] = chunk;

            //var instances = src.Props.Length > 0 ? propInstances.AsSpan(src.Props.Start, src.Props.Length).ToArray() : Array.Empty<PropInstance>();
            //chunk.PropInstances = instances;
        }

        return scenario;
    }



}
