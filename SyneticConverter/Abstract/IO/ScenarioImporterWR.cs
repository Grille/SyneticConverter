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
    private TargetFormat format;
    private IdxFile idx;
    private LvlFile lvl;
    private SniFile sni;
    private VtxFile vtx;
    private QadFile qad;
    private SkyFile sky;

    public ScenarioImporterWR(ScenarioVariant target) : base(target)
    {
        format = target.Owner.Game.Target;
        if (!(format == TargetFormat.WR1 || format == TargetFormat.WR2))
            throw new NotImplementedException();

        idx = new();
        lvl = new();
        sni = new();
        vtx = new();
        qad = format switch
        {
            TargetFormat.WR1 => new QadFileWR1(),
            TargetFormat.WR2 => new QadFileWR2(),
        };
        sky = format switch
        {
            TargetFormat.WR1 => null,
            TargetFormat.WR2 => new(),
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
        AssignTerrain();
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
        var mode = target.Owner.Game.Target;
        if (mode == TargetFormat.WR1)
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

    private void AssignTerrain()
    {
        var terrain = target.Terrain;
        var mesh = terrain.Mesh;

        var vertices = mesh.Vertices = new Vertex[vtx.Vertices.Length];
        for (int i = 0; i < vtx.Vertices.Length; i++)
        {
            ref var srcvtx = ref vtx.Vertices[i];
            var dstvtx = vertices[i] = new Vertex();

            dstvtx.Position = srcvtx.Position;
            dstvtx.UV = srcvtx.UV;
            dstvtx.Normal = new Vector4(srcvtx.Normal.B / 255f, srcvtx.Normal.G / 255f, srcvtx.Normal.R / 255f, srcvtx.Normal.A / 255f);
            dstvtx.Blending = new Vector3(srcvtx.Blend.B, srcvtx.Blend.G, srcvtx.Blend.R);
            dstvtx.Color = srcvtx.Color;
            dstvtx.Shadow = srcvtx.Blend.Shadow;
        }

        mesh.Indecies = new int[idx.Indices.Length];
        for (int i = 0; i < idx.Indices.Length; i++)
        {
            mesh.Indecies[i] = idx.Indices[i];

            if (mesh.Indecies[i] > vertices.Length)
                throw new Exception();
        }



        mesh.PolyRegion = new PolyRegion[qad.Poly.Length];
        for (int i = 0; i < qad.Poly.Length; i++)
        {
            mesh.PolyRegion[i] = new PolyRegion()
            {
                Offset = qad.Poly[i].FirstPoly,
                Count = qad.Poly[i].NumPoly,
                Material = target.WorldMaterials[qad.Poly[i].SurfaceID],
            };
        }

        mesh.Poligons = GenerateTriangles();

        int maxc = 0;
        int max = mesh.Vertices.Length;

        for (int i = 0; i < mesh.Poligons.Length; i++)
        {
            ref var poly = ref mesh.Poligons[i];

            maxc = Math.Max(maxc, Math.Max(Math.Max(poly.X, poly.Y), poly.Z));

            //continue;

            if (poly.X < 0 || poly.Y < 0 || poly.Z < 0)
            {
                throw new Exception();
            }
            if (poly.X > max || poly.Y > max || poly.Z > max)
            {
                poly.X = 0; poly.Y = 0; poly.Z = 0;
                throw new Exception();
            }

        }
    }

    private Vector3Int[] GenerateTriangles()
    {
        var indices = idx.Indices;
        var result = new Vector3Int[indices.Length / 3];


        int pos = 0;
        int xt = 0;
        int preXT = 0;
        for (int iz = 0; iz < qad.Head.BlocksZ; iz++)
        {
            for (int ix = 0; ix < qad.Head.BlocksX; ix++)
            {
                var block = qad.Blocks[iz, ix];
                if (block.Chunk65k != preXT)
                {
                    preXT = block.Chunk65k;
                    xt += vtx.VtxQty[block.Chunk65k - 1];
                }

                int begin = block.FirstPoly;
                int end = begin + block.NumPoly;
                for (int i = begin; i < end; i++)
                {
                    result[i].X = indices[pos + 0] + xt;
                    result[i].Y = indices[pos + 1] + xt;
                    result[i].Z = indices[pos + 2] + xt;

                    pos += 3;
                }
            }
        }

        return result;
    }
    
}
