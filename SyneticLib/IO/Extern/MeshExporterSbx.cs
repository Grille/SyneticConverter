using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using GGL.IO;

namespace SyneticLib.IO.Extern;
public class MeshExporterSbx : MeshExporter
{
    public Vector3 PositionMultiplier = Vector3.One;
    public MeshExporterSbx(Mesh target) : base(target)
    {

    }

    protected override void OnSave()
    {
        using var fs = new FileStream(path, FileMode.Create);
        using var bw = new BinaryViewWriter(fs);

        bw.Write(0);

        for (var i = 0; i < target.Vertices.Length; i++)
        {
            var vertex = target.Vertices[i];
            var pos = vertex.Position * PositionMultiplier;
            var normal = vertex.Normal;
            var uv = vertex.UV0;
            var blend = vertex.Blending;
            var color = vertex.LightColor;
            var shadow = vertex.Shadow;

            bw.Write(pos);
            bw.Write(normal);
            bw.Write(uv);
            bw.Write(blend);
            bw.Write(color);
            bw.Write(shadow);
        }

        for (var i = 0; i < target.Poligons.Length; i++)
        {
            var poly = target.Poligons[i];
            bw.Write(poly);
        }
        /*
        for (var i = 0; i < target.MaterialRegion.Length; i++)
        {
            var region = target.MaterialRegion[i];
            bw.Write(region.Offset);
            bw.Write(region.Count);
            bw.Write(target.Materials.IndexOf((TerrainMaterial)region.Material));
        }

        for (var i = 0; i < target.Materials.Count; i++)
        {
            var mat = target.Materials[i];

            bw.WriteString(mat.Name, LengthPrefix.Byte);
            bw.Write((byte)mat.Mode);
            bw.Write(mat.Texture0.Id);
            bw.Write(mat.Texture1.Id);
            bw.Write(mat.Texture2.Id);
            /*
            bw.Write(mat.Tex0.Transform);
            bw.Write(mat.Tex1.Transform);
            bw.Write(mat.Tex2.Transform);
            
            bw.Write(mat.CastShadown);
            bw.Write(mat.Grass);
            bw.Write(mat.Enlite);
        }
    


        /*
        
        foreach (var poly in target.Poligons)
        {
            sw.WriteLine($"f {poly.X + 1} {poly.Y + 1} {poly.Z + 1}");
        }
        */



    }

}
