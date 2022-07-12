﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;
using SyneticLib.IO.Synetic.Files;

namespace SyneticLib.IO.Synetic;
public class ScenarioImporterWR : ScenarioImporter
{
    private GameVersion format;
    private SynFile syn;
    private IdxFile idx;
    private LvlFile lvl;
    private SniFile sni;
    private VtxFile vtx;
    private QadFile qad;
    private SkyFile sky;

    public ScenarioImporterWR(ScenarioVariant target) : base(target)
    {
        format = target.Owner.Game.Version;
        if (!(format == GameVersion.MBWR || format == GameVersion.WR2))
            throw new NotImplementedException();

        syn = new();

        idx = new();
        lvl = new();
        sni = new();
        vtx = new();
        qad = format switch
        {
            GameVersion.MBWR => new QadFileWR1(),
            GameVersion.WR2 => new QadFileWR2(),
        };
        sky = new();
    }

    protected override void OnSeek(string path)
    {
        var synPath = Path.Combine(path, $"V{target.Number}");
        syn.Path = synPath + ".syn";

        var filePath = Path.Combine(path, target.Owner.Name);
        idx.Path = filePath + ".idx";
        lvl.Path = filePath + ".lvl";
        sni.Path = filePath + ".sni";
        vtx.Path = filePath + ".vtx";
        qad.Path = filePath + ".qad";
        sky.Path = filePath + ".sky";
    }

    protected override void OnLoad()
    {
        if (idx.Exists())
        {
            idx.Load();
            lvl.Load();
            sni.Load();
            vtx.Load();
            qad.Load();
            if (sky.Exists())
                sky.Load();

            return;
        }
        else if (syn.Exists())
        {
            var archive = new SynArchive(syn);
            archive.Load();

            idx.ReadFromArchive(archive);
            lvl.ReadFromArchive(archive);
            sni.ReadFromArchive(archive);
            vtx.ReadFromArchive(archive);
            qad.ReadFromArchive(archive);
            if (archive.FileExists(sky.FileName))
                sky.ReadFromArchive(archive);

            return;
        }

        throw new NotImplementedException();
    }

    protected override void OnInit()
    {
        AssignTextures();
        AssignObjects();
        AssignTerrain(idx, vtx, qad);
        AssignLights();
    }

    public void AssignTextures()
    {
        for (var i = 0; i < qad.TextureName.Length; i++)
        {
            string name = qad.TextureName[i];
            LoadWorldTexture(name);
        }


        /*
        for (int i = 0; i < qad..Length; i++)
        {
            string name = qad.TextureName[i];
            LoadWorldTexture(name);
        }
        */

        for (var i = 0; i < qad.Materials.Length; i++)
        {
            var qmat = qad.Materials[i];
            var mat = new Material();
            var tex0 = target.WorldTextures[qmat.Tex0Id];
            var tex1 = target.WorldTextures[qmat.Tex1Id];
            var tex2 = target.WorldTextures[qmat.Tex2Id];
            var id = i.ToString("X").PadLeft(3, '0');
            mat.Name = $"{id}_{tex0.Name}";
            mat.Mode = (MaterialType)qmat.Mode;
            mat.Tex0.Texture = tex0;
            mat.Tex1.Texture = tex1;
            mat.Tex2.Texture = tex2;
            mat.Tex0.Transform = qmat.Mat1;
            mat.Tex1.Transform = qmat.Mat2;
            mat.Tex2.Transform = qmat.Mat3;

            target.WorldMaterials.Add(mat);
        }
    }

    private void AssignObjects()
    {
        var mode = target.Owner.Game.Version;
        if (mode == GameVersion.MBWR)
        {
            var sqad = (QadFileWR1)qad;
            for (var i = 0; i < qad.Head.PropClassCount; i++)
            {
                var name = qad.PropObjNames[i];
                var data = sqad.PropClasses[i];

                target.PropClasses.Add(new PropClass(name, target.PropMeshes.TextureFolder)
                {

                }); ;
            }
        }
        else
        {
            var sqad = (QadFileWR2)qad;
            for (var i = 0; i < qad.Head.PropClassCount; i++)
            {
                var name = qad.PropObjNames[i];
                var data = sqad.PropClasses[i];

                var prop = new PropClass(name, target.PropMeshes.TextureFolder);
                var path = Path.Combine(target.RootDir, "Objects", name + ".mox");
                if (File.Exists(path))
                    prop.Mesh.ImportFromMox(path);

                target.PropClasses.Add(prop);
            }
        }

        for (var i = 0; i < qad.Head.PropInstanceCount; i++)
        {
            var propIntanceInfo = qad.PropInstances[i];
            var propClass = target.PropClasses[propIntanceInfo.ClassId];
            target.PropInstances.Add(new PropInstance()
            {
                Class = propClass,
                Position = propIntanceInfo.Position,
            });
        }
    }

    private void AssignLights()
    {
        var mode = target.Owner.Game.Version;
        if (mode == GameVersion.MBWR)
        {
        }
        else
        {
            var sqad = (QadFileWR2)qad;
            foreach (var srclight in sqad.Lights)
            {
                var light = new Light();
                light.Color = srclight.Color;
                light.Position = srclight.Matrix.Translation;

                target.Lights.Add(light);
            }
        }
    }


}
