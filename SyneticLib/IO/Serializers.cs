using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.IO;
public static class Serializers
{
    public static class Texture
    {
        public static readonly TextureDdsSerializer Dds = new TextureDdsSerializer();
        public static readonly TexturePtxSerializer Ptx = new TexturePtxSerializer();
        public static readonly TextureTgaSerializer Tga = new TextureTgaSerializer();

        public static readonly SerializerRegistry<SyneticLib.Texture> Registry = new()
        {
            { "dds", Dds },
            { "ptx", Ptx },
            { "tga", Tga }
        };
    }

    public static class Track
    {
        public static readonly TrackTrkSerializer Trk = new TrackTrkSerializer();
    }

    public static class Scenario
    {
        public static readonly ScenarioSyneticSerializer Synetic = new ScenarioSyneticSerializer();
    }

    public static class ScenarioGroup
    {
        public static readonly ScenarioGroupSyneticSerializer Synetic = new ScenarioGroupSyneticSerializer();
    }

    public static class Model
    {
        public static readonly ModelMoxSerializer Mox = new ModelMoxSerializer();
    }

    public static class Mesh
    {
        public static readonly MeshCobSerializer Cob = new MeshCobSerializer();
    }
}
