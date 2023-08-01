using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Numerics;

namespace SyneticLib.IO;
public class LightExportBeamNgJSON
{
    public RessourceList<Light> Lights;
    public Vector3 Scale = Vector3.One;
    public Vector3 Offset = Vector3.Zero;
    public string Parent = "Lights";

    public LightExportBeamNgJSON(RessourceList<Light> lights = null)
    {
        Lights = lights;
    }

    public void Save(string path)
    {
        var sb = new StringBuilder();

        var i = 0;
        foreach (var light in Lights)
        {
            var pos = new Vector3(-light.Position.X * 0.1f + 1228.5f, -light.Position.Z * 0.1f + 1228.5f, light.Position.Y * 0.1f + 180f);


            sb.Append("{");
            sb.Append($"\"__parent\":\"{Parent}\",");
            sb.Append($"\"name\":\"elight_{i++}\",");
            sb.Append($"\"class\":\"SpotLight\",");
            sb.Append($"\"position\":[{pos.X},{pos.Y},{pos.Z}],");
            sb.Append($"\"brightness\":10,");
            sb.Append($"\"color\":[{light.Color.R},{light.Color.G},{light.Color.B}],");
            sb.Append($"\"innerAngle\":0,");
            sb.Append($"\"outerAngle\":170,");
            sb.Append($"\"rotationMatrix\":[1,0,0,0,5.96046448e-08,-0.99999994,0,0.99999994,5.96046448e-08]");
            sb.Append("}");

            sb.AppendLine();
        }

        File.WriteAllText(path, sb.ToString());
    }


}

public static class BeamNGExportUtils
{
    public static void ExportAsBeamNGJSON(this RessourceList<Light> lights, string path)
    {

    }
}
