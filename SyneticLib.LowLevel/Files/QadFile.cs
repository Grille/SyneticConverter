using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using System.IO;
using Grille.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using SyneticLib.Files.Common;
using System.Runtime.CompilerServices;

namespace SyneticLib.Files;

public unsafe class QadFile : BinaryFile
{
    public bool HasCT2Extension = false;
    public bool UseSimpleData = false;
    public bool UseExtendedPropInstance = false;
    public GameVersion MaterialVersion;

    GameVersion _version;
    public GameVersion GameVersion
    {
        get => _version;
        set
        {
            SetFlagsAccordingToVersion(_version);
        }
    }

    public MHeadPrefix HeadPrefix;
    public MHead Head;
    public MHeadExtension HeadExtension;
    public String32[] TextureNames;
    public String32[] BumpTexNames;
    public String32[] PropClassObjNames;
    public MChunk[] Chunks;
    public int[] ChunkDataPtr;
    public ushort[] ChunkData;
    public AbstractMaterialType[] Materials;
    public MPolyRegion[] PolyRegions;
    public MPropInstance[] PropInstances;
    public MGroundPhysics[] GroundPhysics;
    public ushort[] Tex2Ground;
    public MSound[] Sounds;
    public MPropClass[] PropClassInfo;
    public MLight[] Lights;

    public QadFile()
    {
        _version = GameVersion.WR2;

        TextureNames = Array.Empty<String32>();
        BumpTexNames = Array.Empty<String32>();
        PropClassObjNames = Array.Empty<String32>();
        Chunks = Array.Empty<MChunk>();
        ChunkDataPtr = Array.Empty<int>();
        ChunkData = Array.Empty<ushort>();
        Materials = Array.Empty<AbstractMaterialType>();
        PolyRegions = Array.Empty<MPolyRegion>();
        PropInstances = Array.Empty<MPropInstance>();
        GroundPhysics = Array.Empty<MGroundPhysics>();
        Tex2Ground = Array.Empty<ushort>();
        Sounds = Array.Empty<MSound>();
        PropClassInfo = Array.Empty<MPropClass>();
        Lights = Array.Empty<MLight>();
    }

    unsafe void ReadHead(BinaryViewReader br)
    {
        if (HasCT2Extension)
        {
            HeadPrefix = br.Read<MHeadPrefix>();
        }

        Head = br.Read<MHead>();

        if (HasCT2Extension)
        {
            HeadExtension = br.Read<MHeadExtension>();
        }
    }

    public override void Deserialize(BinaryViewReader br)
    {
        ReadHead(br);

        if (Head.FlagX2WR2 != Head.FlagX5WR2)
            throw new InvalidDataException();

        UseSimpleData = Head.FlagX2WR2 == 0;

        AssertBlockCount();
        AssertFileSize(br.Length);

        TextureNames = br.ReadArray<String32>(Head.TexturesFileCount);
        BumpTexNames = br.ReadArray<String32>(Head.BumpTexturesFileCount);
        PropClassObjNames = br.ReadArray<String32>(Head.PropClassCount);

        PropClassInfo = new MPropClass[Head.PropClassCount];
        for (int i = 0; i < Head.PropClassCount; i++)
            PropClassInfo[i] = UseSimpleData ? br.Read<MPropClassSimple>() : br.Read<MPropClass>();

        Chunks = new MChunk[Head.BlockCount];
        for (var i = 0; i < Head.BlockCount; i++)
            Chunks[i] = br.Read<MChunk>();

        var blockX16 = Head.BlockCount * 16;
        ChunkDataPtr = br.ReadArray<int>(blockX16);
        ChunkData = br.ReadArray<ushort>((Head.ColliSize - ChunkDataPtr[0]) / 2);

        PolyRegions = br.ReadArray<MPolyRegion>(Head.PolyRegionCount);

        Materials = new AbstractMaterialType[Head.MaterialCount];
        if (MaterialVersion >= GameVersion.CT2)
            br.ReadToArray(Materials, (MMaterialTypeCT2 a) => (AbstractMaterialType)a);
        else if (MaterialVersion >= GameVersion.C11)
            br.ReadToArray(Materials, (MMaterialTypeC11 a) => (AbstractMaterialType)a);
        else if (MaterialVersion >= GameVersion.WR1)
            br.ReadToArray(Materials, (MMaterialTypeWR a) => (AbstractMaterialType)a);
        else
            throw new InvalidDataException();

        if (HasCT2Extension)
            br.Position += 224 * sizeof(int);

        PropInstances = new MPropInstance[Head.PropInstanceCount];
        if (UseExtendedPropInstance)
            br.ReadItemBytesToArray(PropInstances, sizeof(MPropInstance));
        else
            br.ReadItemBytesToArray(PropInstances, sizeof(MPropInstance) - sizeof(Vector3));

        if (HasCT2Extension)
            br.Position += HeadExtension.StringSkip + HeadExtension.StringCount * sizeof(int) * 2;

        Lights = new MLight[Head.LightCount];
        for (int i = 0; i < Head.LightCount; i++)
            Lights[i] = UseSimpleData ? br.Read<MLightSimple>() : br.Read<MLight>();

        GroundPhysics = br.ReadArray<MGroundPhysics>(Head.GroundPhysicsCount);
        Tex2Ground = br.ReadArray<ushort>(256);
        Sounds = br.ReadArray<MSound>(Head.SoundCount);
    }

    public unsafe override void Serialize(BinaryViewWriter bw)
    {
        bw.LengthPrefix = LengthPrefix.None;

        if (HasCT2Extension)
            throw new InvalidOperationException("CT2+ not supported.");

        if (HasCT2Extension)
        {
            bw.Write(HeadPrefix);
        }
        bw.Write(Head);
        if (HasCT2Extension)
        {
            bw.Write(HeadExtension);
        }

        bw.WriteArray(TextureNames);
        bw.WriteArray(BumpTexNames);
        bw.WriteArray(PropClassObjNames);

        for (int i = 0; i < PropClassInfo.Length; i++)
        {
            if (UseSimpleData)
                bw.Write((MPropClassSimple)PropClassInfo[i]);
            else
                bw.Write(PropClassInfo[i]);
        }

        for (var ix = 0; ix < Head.BlockCount; ix++)
        {
            bw.Write(Chunks[ix]);
        }

        var blockx16 = Head.BlockCount * 16;
        bw.WriteArray(ChunkDataPtr);
        bw.WriteArray(ChunkData);

        bw.WriteArray(PolyRegions);

        if (MaterialVersion >= GameVersion.CT2)
            bw.WriteArray(Materials, (a) => (MMaterialTypeCT2)a);
        if (MaterialVersion >= GameVersion.C11)
            bw.WriteArray(Materials, (a) => (MMaterialTypeC11)a);
        if (MaterialVersion >= GameVersion.WR1)
            bw.WriteArray(Materials, (a) => (MMaterialTypeWR)a);
        else
            throw new InvalidOperationException();

        if (UseExtendedPropInstance)
        {
            bw.WriteItemBytesFromArray(PropInstances, sizeof(MPropInstance));
        }
        else
        {
            bw.WriteItemBytesFromArray(PropInstances, sizeof(MPropInstance) - sizeof(Vector3));
        }

        for (int i = 0; i < Lights.Length; i++)
        {
            if (UseSimpleData)
                bw.Write((MLightSimple)Lights[i]);
            else
                bw.Write(Lights[i]);
        }
        bw.WriteArray(GroundPhysics);
        bw.WriteArray(Tex2Ground);
        bw.WriteArray(Sounds);

        AssertFileSize(bw.Length);
    }

    public GameVersion FindGameVersion(string path)
    {
        using var reader = new BinaryViewReader(path);
        return FindGameVersion(reader);
    }

    public GameVersion FindGameVersion(Stream stream)
    {
        using var reader = new BinaryViewReader(stream);
        return FindGameVersion(reader);
    }

    public unsafe GameVersion FindGameVersion(BinaryViewReader br)
    {
        var stream = br.PeakStream;
        long length = stream.Length;

        bool Check(GameVersion v)
        {
            SetFlagsAccordingToVersion(v);
            var fileSize = CalcFileSize();
            return fileSize == length;
        }

        stream.Seek(0, SeekOrigin.Begin);
        HasCT2Extension = false;
        ReadHead(br);

        if (Check(GameVersion.WR1))
            return GameVersion.WR1;

        if (Check(GameVersion.WR2))
            return GameVersion.WR2;

        if (Check(GameVersion.C11))
            return GameVersion.C11;

        if (Check(GameVersion.CT1))
            return GameVersion.CT1;

        stream.Seek(0, SeekOrigin.Begin);
        HasCT2Extension = true;
        ReadHead(br);

        if (Check(GameVersion.CT2))
            return GameVersion.CT2;

        if (Check(GameVersion.CT3))
            return GameVersion.CT3;

        if (Check(GameVersion.CT4))
            return GameVersion.CT4;

        if (Check(GameVersion.CT5))
            return GameVersion.CT5;

        throw new InvalidDataException("No matching GameVersion found.");
    }

    public void ForceUniqueChecksums()
    {
        int value = 0;
        for (int i = 0; i < Materials.Length; i++)
        {
            Materials[i].Checksums.Checksum0 = value++;
            Materials[i].Checksums.Checksum1 = value++;
            Materials[i].Checksums.Checksum2 = value++;
        }
    }

    public void RecalcMaterialMatrixChecksum()
    {
        var materials = Materials;

        var list = new List<TextureTransform>();

        for (int i = 0; i < materials.Length; i++)
        {
            ref var mat = ref materials[i];
            if (!list.Contains(mat.Matrix0))
                list.Add(mat.Matrix0);

            if (!list.Contains(mat.Matrix1))
                list.Add(mat.Matrix1);

            if (!list.Contains(mat.Matrix2))
                list.Add(mat.Matrix2);
        }

        for (int i = 0; i < materials.Length; i++)
        {
            ref var mat = ref materials[i];

            mat.Checksums.Checksum0 = list.IndexOf(mat.Matrix0);
            mat.Checksums.Checksum1 = list.IndexOf(mat.Matrix1);
            mat.Checksums.Checksum2 = list.IndexOf(mat.Matrix2);
        }

    }

    record class SortContainer<T>(int ID, T Value);
    public void SortMaterials() 
    {
        var materials = Materials;

        var list = new List<SortContainer<AbstractMaterialType>>();

        for (int i = 0; i < materials.Length; i++)
            list.Add(new(i, materials[i]));

        int size = TextureNames.Length;
        list.Sort((a, b) =>
            a.Value.GetSortID(size) - b.Value.GetSortID(size)
        );

        for (int i = 0; i < materials.Length; i++)
            materials[i] = list[i].Value;

        int[] ids = new int[list.Count];
        for (int i = 0; i < list.Count; i++)
            ids[list[i].ID] = i;

        for (int i = 0; i < PolyRegions.Length; i++)
        {
            ref var reg = ref PolyRegions[i];
            reg.SurfaceId1 = (ushort)ids[reg.SurfaceId1];
        }
    }


    public void SetFlagsAccordingToVersion(GameVersion version)
    {
        _version = version;
        MaterialVersion = version;
        UseSimpleData = version <= GameVersion.WR1;
        HasCT2Extension = version >= GameVersion.CT2;
        UseExtendedPropInstance = version >= GameVersion.CT1;
    }

    public void AssertBlockCount()
    {
        if (Head.BlockCountX * Head.BlockCountZ != Head.BlockCount)
            throw new InvalidDataException($"Invalid block count ({Head.BlockCountX} * {Head.BlockCountZ} != {Head.BlockCount}");
    }

    public void AssertFileSize(long length)
    {
        int endPos = CalcFileSize();
        int diff = endPos - (int)length; // + to small, - to big
        if (diff != 0)
            throw new Exception($"Invalid File Size: ({endPos} != {length}) Diff {diff}");

    }

    public unsafe int CalcFileSize()
    {
        int endPos = 0;

        if (HasCT2Extension)
            endPos += sizeof(MHeadPrefix);

        endPos += sizeof(MHead);

        if (HasCT2Extension)
            endPos += sizeof(MHeadExtension);

        endPos += sizeof(String32) * Head.TexturesFileCount;
        endPos += sizeof(String32) * Head.BumpTexturesFileCount;
        endPos += sizeof(String32) * Head.PropClassCount;

        endPos += (UseSimpleData ? sizeof(MPropClassSimple) : sizeof(MPropClass)) * Head.PropClassCount;
        endPos += (UseSimpleData ? sizeof(MLightSimple) : sizeof(MLight)) * Head.LightCount;

        endPos += sizeof(MChunk) * Head.BlockCount;

        endPos += Head.ColliSize;

        endPos += sizeof(MPolyRegion) * Head.PolyRegionCount;

        int materialSize = MaterialVersion >= GameVersion.CT2 ? sizeof(MMaterialTypeCT2) : MaterialVersion >= GameVersion.C11 ? sizeof(MMaterialTypeC11) : sizeof(MMaterialTypeWR);
        endPos += materialSize * Head.MaterialCount;

        if (HasCT2Extension)
            endPos += 224 * sizeof(int);

        endPos += (UseExtendedPropInstance ? sizeof(MPropInstance) : sizeof(MPropInstance) - sizeof(Vector3)) * Head.PropInstanceCount;

        if (HasCT2Extension)
            endPos += HeadExtension.StringSkip + HeadExtension.StringCount * sizeof(int) * 2;

        endPos += sizeof(MGroundPhysics) * Head.GroundPhysicsCount;
        endPos += 2 * 256;
        endPos += sizeof(MSound) * Head.SoundCount;

        return endPos;
    }

    public enum MFileVersion : uint
    {
        None = 0,
        CT2 = 65536,
    }

    public struct MHeadPrefix
    {
        public String4 Head;
        public MFileVersion Version;
    }

    public struct MHead
    {
        public int WidthX, LengthZ, BlockCountX, BlockCountZ, BlockCount, PolyRegionCount;
        public ushort TexturesFileCount, BumpTexturesFileCount;
        public int PropClassCount, PolyCount, MaterialCount, PropInstanceCount, GroundPhysicsCount, ColliSize;
        public ushort LightCount;
        public byte FlagX1, FlagX2WR2, FlagX3, FlagX4, FlagX5WR2, FlagX6;
        public int SoundCount;
    }

    public struct MHeadExtension
    {
        public int StringSkip;
        public int StringCount;
        public fixed int Clear[12];
    }

    public struct MChunk
    {
        public Location2DUInt16 Position;
        public SectionInt32 Poly, PolyRegion;
        public Vector3 Center;
        public float Radius;
        public SectionUInt16 Props, Lights;
        public short Chunk65k, x1;
    }

    public struct MPolyRegion
    {
        public int PolyOffset, PolyCount;
        public ushort SurfaceId1, SurfaceId2;
    }

    public struct MMaterialLayer
    {
        public ushort Tex0Id, Tex1Id, Tex2Id;
        public MMaterialMode Mode;

        public int GetSortID(int count) => Mode * count * count + Tex0Id * count + Tex1Id;
    }

    public struct MMaterialChecksums
    {
        public int Checksum0;
        public int Checksum1;
        public int Checksum2;
    }

    public struct MMaterialTypeWR
    {
        public MMaterialLayer Layer0;
        public TextureTransform Matrix0;
        public TextureTransform Matrix1;
        public TextureTransform Matrix2;
        public MMaterialChecksums Checksums;

        public static explicit operator MMaterialTypeWR(AbstractMaterialType material) => new MMaterialTypeWR()
        {
            Layer0 = material.Layer0,
            Matrix0 = material.Matrix0,
            Matrix1 = material.Matrix1,
            Matrix2 = material.Matrix2,
            Checksums = material.Checksums,
        };
    }

    public struct MMaterialTypeC11
    {
        public MMaterialLayer Layer0;
        public MMaterialLayer Layer1;
        public TextureTransform Matrix0;
        public int A;
        public int B;

        public static explicit operator MMaterialTypeC11(AbstractMaterialType material) => new MMaterialTypeC11()
        {
            Layer0 = material.Layer0,
            Layer1 = material.Layer1,
            Matrix0 = material.Matrix0,
        };
    }

    public struct MMaterialTypeCT2
    {
        public MMaterialLayer Layer0;
        public MMaterialLayer Layer1;
        public TextureTransform Matrix0;
        public float Clear0;
        public int A;
        public int B;

        public static explicit operator MMaterialTypeCT2(AbstractMaterialType material) => new MMaterialTypeCT2()
        {
            Layer0 = material.Layer0,
            Layer1 = material.Layer1,
            Matrix0 = material.Matrix0,
        };
    }

    public struct AbstractMaterialType
    {
        public MMaterialLayer Layer0;
        public MMaterialLayer Layer1;
        public TextureTransform Matrix0;
        public TextureTransform Matrix1;
        public TextureTransform Matrix2;
        public MMaterialChecksums Checksums;

        public static implicit operator AbstractMaterialType(MMaterialTypeWR material) => new AbstractMaterialType()
        {
            Layer0 = material.Layer0,
            Matrix0 = material.Matrix0,
            Matrix1 = material.Matrix1,
            Matrix2 = material.Matrix2,
            Checksums = material.Checksums,
        };

        public static implicit operator AbstractMaterialType(MMaterialTypeC11 material) => new AbstractMaterialType()
        {
            Layer0 = material.Layer0,
            Layer1 = material.Layer1,
            Matrix0 = material.Matrix0,
        };

        public static implicit operator AbstractMaterialType(MMaterialTypeCT2 material) => new AbstractMaterialType()
        {
            Layer0 = material.Layer0,
            Layer1 = material.Layer1,
            Matrix0 = material.Matrix0,
        };

        public int GetSortID(int count) => Layer0.GetSortID(count);
    }

    public struct MPropInstance
    {
        public String32 Name;
        public int ClassId;
        public Vector3 Position;
        public float Angl, Size;
        public Matrix3 Matrix;
        public ushort x1, InShadow;
        public float x5;
        public Vector3 x6;
    }

    public struct MGroundPhysics
    {
        public String64 Name;
        public ushort Dirt, GripF, GripR, Stick, NoiseID, SkidID;
        public fixed byte Padding0[64];
        public ushort NoColliFlag, x8;
        public fixed byte Padding1[12];
    }

    public struct MSound
    {
        public float X, Y, Z;
        public String32 Name;
        public ushort Volume, PlaySpeed, Radius, z4, z5, Delay;
        public fixed byte Misc[12];
    }

    public struct MPropClassSimple
    {
        public ushort Mode;

        public static implicit operator MPropClass(MPropClassSimple a) => new MPropClass()
        {
            Mode = a.Mode,
        };
        public static explicit operator MPropClassSimple(MPropClass a) => new MPropClassSimple()
        {
            Mode = a.Mode,
        };
    }

    public struct MPropClass
    {
        public ushort Mode, Shape, Weight, p4;
        public int x1, x2, x3;
        public String48 HitSound, FallSound;
    }

    public struct MLightSimple
    {
        public Vector3 Position;
        public BgraColor Color;

        public static implicit operator MLight(MLightSimple a) => new MLight()
        {
            Matrix = Matrix4.CreateTranslation(a.Position),
            Color = a.Color,
        };
        public static explicit operator MLightSimple(MLight a) => new MLightSimple()
        {
            Position = a.Matrix.ExtractTranslation(),
            Color = a.Color,
        };
    }

    public struct MMaterialMode
    {
        public ushort Value;
        public static implicit operator ushort(MMaterialMode value) => value.Value;
        public static implicit operator MMaterialMode(ushort value) => Unsafe.As<ushort, MMaterialMode>(ref value);

        public static implicit operator TerrainMaterialTypeMBWR(MMaterialMode value) => (TerrainMaterialTypeMBWR)(ushort)value;
        public static implicit operator TerrainMaterialTypeWR2(MMaterialMode value) => (TerrainMaterialTypeWR2)(ushort)value;
        public static implicit operator TerrainMaterialTypeC11(MMaterialMode value) => (TerrainMaterialTypeC11)(ushort)value;

        public static implicit operator MMaterialMode(TerrainMaterialTypeMBWR value) => (ushort)value;
        public static implicit operator MMaterialMode(TerrainMaterialTypeWR2 value) => (ushort)value;
        public static implicit operator MMaterialMode(TerrainMaterialTypeC11 value) => (ushort)value;
    }

    public struct MLight
    {
        public int Mode;
        public float Size, Offset, Freq;
        public BgraColor Color;
        public byte b1, b2, b3, b4;
        public Matrix4 Matrix;
    }
}
