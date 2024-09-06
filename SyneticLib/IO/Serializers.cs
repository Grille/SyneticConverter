using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;
using SyneticLib.Files.Extra;

namespace SyneticLib.IO;
public static class Serializers
{
    public static class Texture
    {
        public static readonly TextureDdsSerializer Dds = new();
        public static readonly TexturePtxSerializer Ptx = new();
        public static readonly TextureTgaSerializer Tga = new();

        public static readonly SerializerRegistry<SyneticLib.Texture> Registry = new()
        {
            { "dds", Dds },
            { "ptx", Ptx },
            { "tga", Tga }
        };
    }

    public static class Track
    {
        public static readonly TrackTrkSerializer Trk = new();
    }

    public static class Scenario
    {
        public static readonly ScenarioSyneticSerializer Synetic = new();
    }

    public static class ScenarioGroup
    {
        public static readonly ScenarioGroupSyneticSerializer Synetic = new();
    }

    public static class Model
    {
        public static readonly ModelSyneticSerializer Synetic = new();

        public static readonly ModelWavefrontSerializer Wavefront = new();

        public static readonly SerializerRegistry<SyneticLib.Model> Registry = new()
        {
            { "synetic", Synetic },
            { "wavefront", Wavefront },
        };
    }

    public static class Mesh
    {
        public static readonly MeshFileSerializer<CpoFile> Cpo = new();
        public static readonly MeshFileSerializer<CobFile> Cob = new();
        public static readonly MeshFileSerializer<MoxFile> Mox = new();
        public static readonly MeshFileSerializer<WavefrontObjFile> Obj = new();

        public static readonly SerializerRegistry<SyneticLib.Mesh> Registry = new()
        {
            { "cpo", Cpo },
            { "cob", Cob },
            { "mox", Mox },
            { "obj", Obj },
        };
    }
}
