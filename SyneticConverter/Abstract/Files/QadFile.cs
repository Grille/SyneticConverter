using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using GGL.IO;
using System.Runtime.InteropServices;

namespace SyneticConverter;

public abstract class QadFile : SyneticBinFile
{
    public bool HasMagic = false;
    public bool Has56DataBlock = false;

    public MHead Head;
    public String32[] TextureName;
    public String32[] BumpTexName;
    public String32[] PropObjNames;
    public BMlock[,] Blocks;
    public int[] ChunkDataPtr;
    public ushort[][] ChunkData;
    public MMaterialDef[] Materials;
    public MPolygonRegionPtr[] Poly;
    public MObjInstance[] PropInstances;
    public MGround[] Grounds;
    public ushort[] Tex2Ground;
    public MSound[] Sounds;

    public struct MHead
    {
        public int WidthX, LengthZ, BlocksX, BlocksZ, BlocksTotal, TexturesTotal;
        public ushort TexturesFiles, BumpTexturesFiles;
        public int PropClassCount, PolyCount, MaterialCount, PropInstanceCount, GroundTypes, ColliSize;
        public ushort Lights, x1, x2, x3;
        public int Sounds;
    }

    public struct BMlock
    {
        public ushort X, Z;
        public int FirstPoly, NumPoly, FirstTex, NumTex;
        public float CenterX, CenterY, CenterZ, Rad;
        public short FirstObj, NumObj, FirstLight, NumLight;
        public short Chunk65k, x1;
    }

    public struct MPolygonRegionPtr
    {
        public int FirstPoly, NumPoly;
        public ushort SurfaceID, SurfaceID2;
    }

    public struct MMaterialDef
    {
        public ushort Tex0Id, Tex1Id, Tex2Id, Mode;
        public Transform Mat1;
        public Transform Mat2;
        public Transform Mat3;
        public int A;
        public int B;
        public int C;
    }

    public struct MObjInstance
    {
        public String32 Name;
        public int ClassId;
        public Vector3 Position;
        public float Angl, Size;
        public Matrix3x3 Matrix;
        public ushort x1, InShadow;
        public float x5;
    }

    public struct MGround
    {
        public String64 Name;
        public ushort Dirt, GripF, GripR, Stick, NoiseID, SkidID;
        Clear0Type clear0; //empty
        public ushort NoColliFlag, x8;
        Clear1Type clear1; //empty

        [StructLayout(LayoutKind.Sequential, Size = 32 * 2)] struct Clear0Type { }
        [StructLayout(LayoutKind.Sequential, Size = 6 * 2)] struct Clear1Type { }
    }

    public struct MSound
    {
        public float X, Y, Z;
        public String32 Name;
        public ushort Volume, PlaySpeed, Radius, z4, z5, Delay;

        Misc misc;
        [StructLayout(LayoutKind.Sequential, Size = 12)] struct Misc { }
    }
}

public abstract class QadFile<TMPropClass, TMLight> : QadFile  where TMPropClass : unmanaged where TMLight : unmanaged
{
    public TMPropClass[] PropClasses;
    public TMLight[] Lights;

    public unsafe override void Read(BinaryViewReader br)
    {
        if (HasMagic)
            br.Position += 8;

        Head = br.Read<MHead>();

        if (Has56DataBlock)
            br.ReadArray<byte>(56);

        TextureName = br.ReadArray<String32>(Head.TexturesFiles);
        BumpTexName = br.ReadArray<String32>(Head.BumpTexturesFiles);
        PropObjNames = br.ReadArray<String32>(Head.PropClassCount);

        PropClasses = br.ReadArray<TMPropClass>(Head.PropClassCount);

        Blocks = new BMlock[Head.BlocksZ, Head.BlocksX];

        for (var iz = 0; iz < Head.BlocksZ; iz++)
        {
            for (var ix = 0; ix < Head.BlocksX; ix++)
            {
                Blocks[iz, ix] = br.Read<BMlock>();
            }
        }

        var blockX16 = Head.BlocksTotal * 16;

        ChunkDataPtr = new int[blockX16 + 1];
        br.ReadToIList(ChunkDataPtr, 0, blockX16);
        ChunkDataPtr[blockX16] = Head.ColliSize;

        ChunkData = new ushort[blockX16][];
        for (var i = 0; i < blockX16; i++)
        {
            var size = (ChunkDataPtr[i + 1] - ChunkDataPtr[i]) / 2;
            ChunkData[i] = br.ReadArray<ushort>(size);
        }

        Poly = br.ReadArray<MPolygonRegionPtr>(Head.TexturesTotal);
        Materials = br.ReadArray<MMaterialDef>(Head.MaterialCount);
        PropInstances = br.ReadArray<MObjInstance>(Head.PropInstanceCount);
        Lights = br.ReadArray<TMLight>(Head.Lights);
        Grounds = br.ReadArray<MGround>(Head.GroundTypes);
        Tex2Ground = br.ReadArray<ushort>(256);
        Sounds = br.ReadArray<MSound>(Head.Sounds);
    }

    public override void Write(BinaryViewWriter bw)
    {
        bw.DefaultLengthPrefix = LengthPrefix.None;

        bw.Write(Head);

        bw.WriteArray(TextureName);
        bw.WriteArray(BumpTexName);
        bw.WriteArray(PropObjNames);
        bw.WriteArray(PropClasses);

        for (var ix = 0; ix < Head.BlocksX; ix++)
        {
            for (var iz = 0; iz < Head.BlocksZ; iz++)
            {
                bw.Write(Blocks[ix, iz]);
            }
        }

        var blockx16 = Head.BlocksTotal * 16;

        bw.WriteIList(ChunkDataPtr, 0, blockx16);
        for (var i = 0; i < blockx16; i++)
        {
            bw.WriteArray(ChunkData[i]);
        }


        bw.WriteArray(Poly);
        bw.WriteArray(Materials);
        bw.WriteArray(PropInstances);
        bw.WriteArray(Lights);
        bw.WriteArray(Grounds);
        bw.WriteArray(Tex2Ground);
        bw.WriteArray(Sounds);
    }
}
