using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.IO;

using OpenTK.Mathematics;

using SyneticLib.Files.Common;

namespace SyneticLib.Files.Extra;

public class SbeFile : BinaryFile, IVertexData, IIndexData
{
    public const int Version = 1;

    public const int TextureCount = 8;
    public Vertex[] Vertecis { get; set; }

    public IdxTriangleInt32[] Triangles { get; set; }

    public SbeMaterial[] Materials { get; set; }

    public SbeModel[] Models { get; set; }

    public SbeFile()
    {
        Vertecis = Array.Empty<Vertex>();
        Triangles = Array.Empty<IdxTriangleInt32>();

        Materials = Array.Empty<SbeMaterial>();
        Models = Array.Empty<SbeModel>();
    }

    public override void Deserialize(BinaryViewReader br)
    {
        throw new NotImplementedException();
    }

    public override void Serialize(BinaryViewWriter bw)
    {
        // Head
        bw.WriteInt32(Version);
        bw.WriteInt32(TextureCount);

        bw.WriteInt32(Vertecis.Length);
        bw.WriteInt32(Triangles.Length);
        bw.WriteInt32(Materials.Length);
        bw.WriteInt32(Models.Length);

        for (int i = 0; i < Vertecis.Length; i++)
        {
            var vtx = Vertecis[i];
            vtx.UV0 = new Vector2(vtx.UV0.X, 1 - vtx.UV0.Y);
            vtx.UV1 = new Vector2(vtx.UV1.X, 1 - vtx.UV1.Y);
            bw.Write(vtx);
        }
        bw.WriteArray(Triangles, LengthPrefix.None);

        bw.LengthPrefix = LengthPrefix.Int32;

        foreach (var material in Materials)
        {
            bw.WriteString(material.Name);
            bw.WriteInt32((int)material.Type);

            foreach (var texture in material.Textures)
            {
                bw.WriteString(texture);
            }
        }

        foreach (var model in Models)
        {
            bw.WriteString(model.Name);
            bw.WriteInt32(model.Regions.Length);

            foreach (var region in model.Regions)
            {
                bw.WriteString(region.Material);
                bw.WriteInt32(region.Offset);
                bw.WriteInt32(region.Length);
            }
        }
    }

    public record class SbeModel(string Name, SbeRegion[] Regions);

    public struct SbeRegion
    {
        public string Material;
        public int Offset;
        public int Length;
    }

    public enum SbeMaterialType : int
    {
        L1UV,
        L3Terrain,
    }

    public class SbeMaterial
    {
        public string Name;
        public SbeMaterialType Type;

        public readonly string[] Textures;

        public SbeMaterial()
        {
            Name = string.Empty;
            Textures = new string[TextureCount];
            for (int i = 0; i < Textures.Length; i++)
            {
                Textures[i] = string.Empty;
            }
        }
    }
}
