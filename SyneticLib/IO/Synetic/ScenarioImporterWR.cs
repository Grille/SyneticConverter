using System;
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
        const string errmsg = $"Version needs to be between MBWR - WR2.";

        format = target.Version;
        if (!(format == GameVersion.MBWR || format == GameVersion.WR2))
            throw new NotImplementedException(errmsg);

        syn = new();

        idx = new();
        lvl = new();
        sni = new();
        vtx = new();
        qad = format switch
        {
            GameVersion.MBWR => new QadFileWR1(),
            GameVersion.WR2 => new QadFileWR2(),
            _ => throw new NotImplementedException(errmsg)
        };
        sky = new();

        var synPath = Path.Combine(target.SourcePath, $"V{target.IdNumber}");
        syn.Path = synPath + ".syn";

        var filePath = Path.Combine(target.SourcePath, target.Parent.FileName);
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
        /*
        for (var i = 0; i < qad.TextureName.Length; i++)
        {
            string name = qad.TextureName[i];
            LoadWorldTexture(name);
        }
        */
        


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
            var tex0 = target.TerrainTextures[qmat.Tex0Id];
            var tex1 = target.TerrainTextures[qmat.Tex1Id];
            var tex2 = target.TerrainTextures[qmat.Tex2Id];
            var id = i.ToString("X").PadLeft(3, '0');
            mat.Name = $"{id}_{tex0.FileName}";
            mat.Mode = (TerrainMaterialType)qmat.Mode;
            mat.Texture0 = tex0;
            mat.Texture1 = tex1;
            mat.Texture2 = tex2;
            /*
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

                target.PropClasses.Add(new PropClass(target,name)
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

                prop.DataState = DataState.Loaded;

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

        target.PropClasses.DataState = DataState.Loaded;

    }

    private void AssignLights()
    {
        var mode = target.Version;
        if (mode == GameVersion.MBWR)
        {
        }
        else
        {
            var sqad = (QadFileWR2)qad;
            foreach (var srclight in sqad.Lights)
            {
                var light = new Light(target);
                light.Color = srclight.Color;
                light.Position = srclight.Matrix.Translation;

                target.Lights.Add(light);
            }
        }
        target.Lights.DataState = DataState.Loaded;
    }


}
