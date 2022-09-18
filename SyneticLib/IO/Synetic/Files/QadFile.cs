using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;
using GGL.IO;
using System.Runtime.InteropServices;

namespace SyneticLib.IO.Synetic.Files;

public class QadFile : SyneticBinaryFile
{
    public bool Has8ByteMagic = false;
    public bool Has56ByteBlock = false;
    public bool UseSimpleData = false;

    public MHead Head;
    public String32[] TextureNames;
    public String32[] BumpTexName;
    public String32[] PropObjNames;
    public MChunk[] Blocks;
    public int[] ChunkDataPtr;
    public ushort[][] ChunkData;
    public MMaterialDef[] Materials;
    public MPolygonRegionPtr[] MaterialRegions;
    public MObjInstance[] PropInstances;
    public MGroundPhysics[] Grounds;
    public ushort[] Tex2Ground;
    public MSound[] Sounds;
    public MObjProp[] PropClasses;
    public MLight[] Lights;

    public struct MHead
    {
        public String4 Head;
        public int Version;
        public int WidthX, LengthZ, BlocksX, BlocksZ, BlocksTotal, TexturesTotal;
        public ushort TexturesFiles, BumpTexturesFiles;
        public int PropClassCount, PolyCount, MaterialCount, PropInstanceCount, GroundTypes, ColliSize;
        public ushort Lights, x1, x2, x3;
        public int Sounds;
    }

    public struct MChunk
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

    public struct MGroundPhysics
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


    public struct MObjPropSimple
    {
        public ushort Mode;

        public static implicit operator MObjProp(MObjPropSimple a) => new MObjProp()
        {
            Mode = a.Mode,
        };
        public static explicit operator MObjPropSimple(MObjProp a) => new MObjPropSimple()
        {
            Mode = a.Mode,
        };
    }

    public struct MLightSimple
    {
        public Vector3 Position;
        public BgraColor Color;

        public static implicit operator MLight(MLightSimple a) => new MLight()
        {
            Matrix = Matrix4x4.CreateTranslation(a.Position),
            Color = a.Color,
        };
        public static explicit operator MLightSimple(MLight a) => new MLightSimple()
        {
            Position = a.Matrix.Translation,
            Color = a.Color,
        };
    }

    public struct MObjProp
    {
        public ushort Mode, Shape, Weight, p4;
        public int x1, x2, x3;
        public String48 HitSound, FallSound;
    }

    public struct MLight
    {
        public int Mode;
        public float Size, Offset, Freq;
        public BgraColor Color;
        public byte b1, b2, b3, b4;
        public Matrix4x4 Matrix;
    }

    public unsafe override void ReadFromView(BinaryViewReader br)
    {

        fixed (void* ptr = &Head)
        {
            int ptroffset = Has8ByteMagic ? 0 : 8;
            br.ReadToPtr((byte*)ptr + ptroffset, sizeof(MHead) - ptroffset);
        }

        if (Head.BlocksX * Head.BlocksZ != Head.BlocksTotal)
            throw new InvalidDataException($"Invalid block count ({Head.BlocksX} * {Head.BlocksZ}) != {Head.BlocksTotal}");

        if (Has56ByteBlock)
            br.ReadArray<byte>(56);

        TextureNames = br.ReadArray<String32>(Head.TexturesFiles);
        BumpTexName = br.ReadArray<String32>(Head.BumpTexturesFiles);
        PropObjNames = br.ReadArray<String32>(Head.PropClassCount);

        PropClasses = new MObjProp[Head.PropClassCount];
        for (int i = 0; i < Head.PropClassCount; i++)
            PropClasses[i] = UseSimpleData ? br.Read<MObjPropSimple>() : br.Read<MObjProp>();

        Blocks = new MChunk[Head.BlocksTotal];
        for (var i = 0; i < Head.BlocksTotal; i++)
             Blocks[i] = br.Read<MChunk>();

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

        
        MaterialRegions = br.ReadArray<MPolygonRegionPtr>(Head.TexturesTotal);
        Materials = br.ReadArray<MMaterialDef>(Head.MaterialCount);
        PropInstances = br.ReadArray<MObjInstance>(Head.PropInstanceCount);

        Lights = new MLight[Head.Lights];
        for (int i = 0; i < Head.Lights; i++)
            Lights[i] = UseSimpleData ? br.Read<MLightSimple>() : br.Read<MLight>();

        Grounds = br.ReadArray<MGroundPhysics>(Head.GroundTypes);
        Tex2Ground = br.ReadArray<ushort>(256);
        Sounds = br.ReadArray<MSound>(Head.Sounds);
    }

    public override void WriteToView(BinaryViewWriter bw)
    {
        bw.DefaultLengthPrefix = LengthPrefix.None;

        bw.Write(Head);

        bw.WriteArray(TextureNames);
        bw.WriteArray(BumpTexName);
        bw.WriteArray(PropObjNames);

        for (int i = 0; i < PropClasses.Length; i++)
        {
            if (UseSimpleData)
                bw.Write((MObjPropSimple)PropClasses[i]);
            else
                bw.Write(PropClasses[i]);
        }

        for (var ix = 0; ix < Head.BlocksTotal; ix++)
        {
            bw.Write(Blocks[ix]);
        }

        var blockx16 = Head.BlocksTotal * 16;

        bw.WriteIList(ChunkDataPtr, 0, blockx16);
        for (var i = 0; i < blockx16; i++)
        {
            bw.WriteArray(ChunkData[i]);
        }

        bw.WriteArray(MaterialRegions);
        bw.WriteArray(Materials);
        bw.WriteArray(PropInstances);
        for (int i = 0; i < Lights.Length; i++)
        {
            if (UseSimpleData)
                bw.Write((MLightSimple)Lights[i]);
            else
                bw.Write(Lights[i]);
        }
        bw.WriteArray(Grounds);
        bw.WriteArray(Tex2Ground);
        bw.WriteArray(Sounds);
    }
}
