﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using System.IO;
using GGL.IO;
using System.Runtime.InteropServices;
using System.Drawing;

namespace SyneticLib.LowLevel.Files;

public unsafe class QadFile : BinaryFile
{
    public bool Has8ByteMagic = false;
    public bool Has56ByteBlock = false;
    public bool UseSimpleData = false;
    public bool UseMaterialTypeCT = false;

    public MHead Head;
    public String32[] TextureNames;
    public String32[] BumpTexNames;
    public String32[] PropClassObjNames;
    public MChunk[] Chunks;
    public int[] ChunkDataPtr;
    public ushort[][] ChunkData;
    public MMaterialTypeWR[] MaterialsWR;
    public MMaterialTypeCT[] MaterialsCT;
    public MPolyRegion[] PolyRegions;
    public MPropInstance[] PropInstances;
    public MGroundPhysics[] GroundPhysics;
    public ushort[] Tex2Ground;
    public MSound[] Sounds;
    public MPropClass[] PropClassInfo;
    public MLight[] Lights;

    public unsafe override void ReadFromView(BinaryViewReader br)
    {

        fixed (void* ptr = &Head)
        {
            int ptroffset = Has8ByteMagic ? 0 : 8;
            br.ReadToPtr((byte*)ptr + ptroffset, sizeof(MHead) - ptroffset);
        }

        assertBlockCount();
        assertFileSize(br.Length);

        if (Has56ByteBlock)
            br.ReadArray<byte>(56);

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
        ChunkDataPtr = new int[blockX16 + 1];
        br.ReadToIList(ChunkDataPtr, 0, blockX16);
        ChunkDataPtr[blockX16] = Head.ColliSize;

        ChunkData = new ushort[blockX16][];
        for (var i = 0; i < blockX16; i++)
        {
            var size = (ChunkDataPtr[i + 1] - ChunkDataPtr[i]) / 2;
            ChunkData[i] = br.ReadArray<ushort>(size);
        }


        PolyRegions = br.ReadArray<MPolyRegion>(Head.PolyRegionCount);

        if (UseMaterialTypeCT)
        {
            MaterialsCT = br.ReadArray<MMaterialTypeCT>(Head.MaterialCount);
        }
        else
        {
            MaterialsWR = br.ReadArray<MMaterialTypeWR>(Head.MaterialCount);
        }

        PropInstances = br.ReadArray<MPropInstance>(Head.PropInstanceCount);

        Lights = new MLight[Head.LightCount];
        for (int i = 0; i < Head.LightCount; i++)
            Lights[i] = UseSimpleData ? br.Read<MLightSimple>() : br.Read<MLight>();

        GroundPhysics = br.ReadArray<MGroundPhysics>(Head.GroundPhysicsCount);
        Tex2Ground = br.ReadArray<ushort>(256);
        Sounds = br.ReadArray<MSound>(Head.SoundCount);
    }

    public unsafe override void WriteToView(BinaryViewWriter bw)
    {
        bw.LengthPrefix = LengthPrefix.None;

        fixed (void* ptr = &Head)
        {
            int ptroffset = Has8ByteMagic ? 0 : 8;
            bw.WriteFromPtr((byte*)ptr + ptroffset, sizeof(MHead) - ptroffset);
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

        bw.WriteIList(ChunkDataPtr, 0, blockx16);
        for (var i = 0; i < blockx16; i++)
        {
            bw.WriteArray(ChunkData[i]);
        }

        bw.WriteArray(PolyRegions);

        if (UseMaterialTypeCT)
        {
            bw.WriteArray(MaterialsCT);
        }
        else
        {
            bw.WriteArray(MaterialsWR);
        }

        bw.WriteArray(PropInstances);
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

        assertFileSize(bw.Length);
    }

    public void RecalcMaterialChecksum()
    {
        if (UseMaterialTypeCT)
            throw new NotImplementedException();
        else
            RecalcMaterialChecksum(MaterialsWR);
    }

    public void RecalcMaterialChecksum(MMaterialTypeWR[] materials)
    {
        var list = new List<Transform>();

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

            mat.MatrixChecksum0 = list.IndexOf(mat.Matrix0);
            mat.MatrixChecksum1 = list.IndexOf(mat.Matrix1);
            mat.MatrixChecksum2 = list.IndexOf(mat.Matrix2);
        }

    }

    public void SortMaterials()
    {
        if (UseMaterialTypeCT)
            SortMaterials(MaterialsCT);
        else
            SortMaterials(MaterialsWR);
    }

    record class SortContainer<T>(int ID, T Value);
    public void SortMaterials<T>(T[] materials) where T : IMaterialType
    {
        var list = new List<SortContainer<T>>();

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
        UseSimpleData = version <= GameVersion.WR1;
        Has8ByteMagic = version >= GameVersion.CT2;
        Has56ByteBlock = version >= GameVersion.CT2;
        UseMaterialTypeCT = version >= GameVersion.C11;
    }

    private void assertBlockCount()
    {
        if (Head.BlockCountX * Head.BlockCountZ != Head.BlockCount)
            throw new InvalidDataException($"Invalid block count ({Head.BlockCountX} * {Head.BlockCountZ} != {Head.BlockCount}");
    }

    private void assertFileSize(long length)
    {
        int endPos = calcFileSize();
        int diff = endPos - (int)length; // + to small, - to big
        if (diff != 0)
            throw new Exception($"Invalid File Size: ({endPos} != {length}) Diff {diff}");

    }

    private unsafe int calcFileSize()
    {
        int endPos = 0;

        endPos += sizeof(MHead) - (Has8ByteMagic ? 0 : 8);

        if (Has56ByteBlock)
            endPos += 56;

        endPos += sizeof(String32) * Head.TexturesFileCount;
        endPos += sizeof(String32) * Head.BumpTexturesFileCount;
        endPos += sizeof(String32) * Head.PropClassCount;

        endPos += (UseSimpleData ? sizeof(MPropClassSimple) : sizeof(MPropClass)) * Head.PropClassCount;
        endPos += (UseSimpleData ? sizeof(MLightSimple) : sizeof(MLight)) * Head.LightCount;

        endPos += sizeof(MChunk) * Head.BlockCount;

        endPos += Head.ColliSize;

        endPos += sizeof(MPolyRegion) * Head.PolyRegionCount;
        endPos += (UseMaterialTypeCT ? sizeof(MMaterialTypeCT) : sizeof(MMaterialTypeWR)) * Head.MaterialCount;
        endPos += sizeof(MPropInstance) * Head.PropInstanceCount;


        endPos += sizeof(MGroundPhysics) * Head.GroundPhysicsCount;
        endPos += 2 * 256;
        endPos += sizeof(MSound) * Head.SoundCount;

        return endPos;
    }

    public struct MHead
    {
        public String4 Head;
        public int Version;
        public int WidthX, LengthZ, BlockCountX, BlockCountZ, BlockCount, PolyRegionCount;
        public ushort TexturesFileCount, BumpTexturesFileCount;
        public int PropClassCount, PolyCount, MaterialCount, PropInstanceCount, GroundPhysicsCount, ColliSize;
        public ushort LightCount;
        public byte FlagX1, FlagX2, FlagX3, FlagX4, FlagX5, FlagX6;
        public int SoundCount;
    }

    public struct MChunk
    {
        public ushort PosX, PosZ;
        public int PolyOffset, PolyCount, PolyRegionOffset, PolyRegionCount;
        public Vector3 Center;
        public float Radius;
        public short PropOffset, PropCount, LightOffset, LightCount;
        public short Chunk65k, x1;
    }

    public struct MPolyRegion
    {
        public int PolyOffset, PolyCount;
        public ushort SurfaceId1, SurfaceId2;
    }

    public interface IMaterialType
    {
        public int GetSortID(int count);
    }

    public struct MMaterialTypeWR : IMaterialType
    {
        public ushort Tex0Id, Tex1Id, Tex2Id;
        public EMaterialMode Mode;
        public Transform Matrix0;
        public Transform Matrix1;
        public Transform Matrix2;
        public int MatrixChecksum0;
        public int MatrixChecksum1;
        public int MatrixChecksum2;
        public int GetSortID(int count) => (Mode) * count * count + (Tex0Id) * count + (Tex1Id);
    }
    public struct MMaterialTypeCT : IMaterialType
    {
        public ushort L0Tex0Id, L0Tex1Id, L0Tex2Id, L0Mode;
        public ushort L1Tex0Id, L1Tex1Id, L1Tex2Id, L1Mode;
        public Transform Mat1;
        public int A;
        public int B;

        public int GetSortID(int count) => 0;
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

    public struct MLight
    {
        public int Mode;
        public float Size, Offset, Freq;
        public BgraColor Color;
        public byte b1, b2, b3, b4;
        public Matrix4 Matrix;
    }

    public record struct EMaterialMode
    {
        public ushort Value;

        public readonly static EMaterialMode WR2Terrain = 0;
        public readonly static EMaterialMode WR1Terrain = 0;

        public readonly static EMaterialMode WR2Water = 0;
        public readonly static EMaterialMode WR1Water = 0;

        public readonly static EMaterialMode WR2Masked = 0;
        public readonly static EMaterialMode WR1Masked = 0;

        public static implicit operator ushort(EMaterialMode x) => x.Value;
        public static implicit operator EMaterialMode(ushort x) => new EMaterialMode() { Value = x };
    }
}
