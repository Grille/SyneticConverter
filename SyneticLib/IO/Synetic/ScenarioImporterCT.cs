﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;
using SyneticLib.IO.Synetic.Files;

namespace SyneticLib.IO.Synetic;
public class ScenarioImporterCT : ScenarioImporter
{
    private GameVersion format;
    GeoFile geo;
    LvlFile lvl;
    QadFile qad;
    SniFile sni;

    public ScenarioImporterCT(ScenarioVariant target) : base(target)
    {
        format = target.Version;
        if (format < GameVersion.C11)
            throw new NotImplementedException();

        geo = new();
        lvl = new();
        sni = new();
        qad = format >= GameVersion.CT2 ? new QadFileCT2() : new QadFileWR2();

        if (format >= GameVersion.CT5)
            geo.HasX16VertexBlock = true;

        var filePath = Path.Combine(target.SourcePath, target.Parent.FileName);
        geo.Path = filePath + ".geo";
        lvl.Path = filePath + ".lvl";
        qad.Path = filePath + ".qad";
        sni.Path = filePath + ".sni";
    }


    protected override void OnLoad()
    {
        geo.Load();
        lvl.Load();
        qad.Load();
        if (sni.Exists())
            sni.Load();
    }

    protected override void OnInit()
    {
        AssignTextures();
        //AssignObjects();
        AssignTerrain(geo, geo, qad);
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
            var mat = new TerrainMaterial(target);
            //var tex0 = target.WorldTextures[qmat.Tex0Id];
            //var tex1 = target.WorldTextures[qmat.Tex1Id];
            //var tex2 = target.WorldTextures[qmat.Tex2Id];
            var id = i.ToString("X").PadLeft(3, '0');
            mat.Name = $"{id}_{"j"}";
            /*
            mat.Mode = (MaterialType)qmat.Mode;
            mat.Tex0.Texture = tex0;
            mat.Tex1.Texture = tex1;
            mat.Tex2.Texture = tex2;
            mat.Tex0.Transform = qmat.Mat1;
            mat.Tex1.Transform = qmat.Mat2;
            mat.Tex2.Transform = qmat.Mat3;
            */

            target.TerrainMaterials.Add(mat);
        }
    }

    private void AssignObjects()
    {
        var mode = target.Version;
        if (mode == GameVersion.MBWR)
        {
            var sqad = (QadFileWR1)qad;
            for (var i = 0; i < qad.Head.PropClassCount; i++)
            {
                var name = qad.PropObjNames[i];
                var data = sqad.PropClasses[i];

                target.PropClasses.Add(new PropClass(target, name)
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

                var prop = new PropClass(target,name);
                var path = Path.Combine(target.SourcePath, "Objects", name + ".mox");
                target.PropClasses.Add(prop);
            }
        }

        for (var i = 0; i < qad.Head.PropInstanceCount; i++)
        {
            var propIntanceInfo = qad.PropInstances[i];
            var propClass = target.PropClasses[propIntanceInfo.ClassId];
            target.PropInstances.Add(new PropInstance(target,propClass)
            {
                Class = propClass,
                Position = propIntanceInfo.Position,
            });
        }
    }

}
