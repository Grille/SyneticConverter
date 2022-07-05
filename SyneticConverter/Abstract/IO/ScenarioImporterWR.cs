using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;

namespace SyneticConverter;
public class ScenarioImporterWR : ScenarioImporter
{
    private GameVersion format;
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

        idx = new();
        lvl = new();
        sni = new();
        vtx = new();
        qad = format switch
        {
            GameVersion.MBWR => new QadFileWR1(),
            GameVersion.WR2 => new QadFileWR2(),
        };
        sky = format switch
        {
            GameVersion.MBWR => null,
            GameVersion.WR2 => new(),
        };
    }

    public override void Load()
    {
        var filePath = Path.Combine(target.RootDir, target.Owner.Name);

        idx.Load(filePath + ".idx");
        lvl.Load(filePath + ".lvl");
        sni.Load(filePath + ".sni");
        vtx.Load(filePath + ".vtx");
        qad.Load(filePath + ".qad");
        if (sky != null)
            sky.Load(filePath + ".sky");
    }

    public override void Assign()
    {
        AssignTextures();
        AssignObjects();
        AssignTerrain(idx, vtx, qad);
    }

    public void AssignTextures()
    {
        for (int i = 0; i < qad.TextureName.Length; i++)
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

        for (int i = 0; i< qad.Materials.Length; i++)
        {
            var qmat = qad.Materials[i];
            var mat = new Material();
            var tex0 = target.WorldTextures[qmat.Tex0Id];
            var tex1 = target.WorldTextures[qmat.Tex1Id];
            var tex2 = target.WorldTextures[qmat.Tex2Id];
            string id = i.ToString("X").PadLeft(3, '0');
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
            for (int i = 0; i < qad.Head.PropClassCount; i++)
            {
                var name = qad.PropObjNames[i];
                var data = sqad.PropClasses[i];

                target.PropClasses.Add(new PropClass(name, target.PropTextures)
                {
                    
                }); ;
            }
        }
        else
        {
            var sqad = (QadFileWR2)qad;
            for (int i = 0; i < qad.Head.PropClassCount; i++)
            {
                var name = qad.PropObjNames[i];
                var data = sqad.PropClasses[i];

                var prop = new PropClass(name, target.PropTextures);
                string path = Path.Combine(target.RootDir, "Objects", name + ".mox");
                if (File.Exists(path))
                    prop.Mesh.ImportMox(path);

                target.PropClasses.Add(prop);
            }
        }

        for (int i = 0; i < qad.Head.PropInstanceCount; i++)
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


    
}
