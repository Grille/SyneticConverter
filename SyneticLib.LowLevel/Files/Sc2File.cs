using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grille.IO;
using SyneticLib.Files.Common;
using Grille.IO.Interfaces;
using Microsoft.VisualBasic;

namespace SyneticLib.Files;
public class Sc2File : BinaryFile
{
    public const uint Magic = 20075095;

    public string FileName;
    public string DisplayName;
    public string BackgroundImage;
    public string FlagImage;

    public string Author;
    public string Converter;
    public string Contact;
    public string Comment;

    public ushort U0;
    public ushort FreeRideID;

    public Track[] Tracks = Array.Empty<Track>();

    public Sc2File()
    {
        FileName = string.Empty;
        DisplayName = string.Empty;
        BackgroundImage = string.Empty;
        FlagImage = string.Empty;

        Author = string.Empty;
        Converter = string.Empty;
        Contact = string.Empty;
        Comment = string.Empty;
    }

    public override void Deserialize(BinaryViewReader br)
    {
        uint magic = br.ReadUInt32();
        if (magic != Magic)
        {
            throw new InvalidDataException();
        }

        U0 = br.ReadUInt16();

        FileName = br.ReadKromString();
        BackgroundImage = br.ReadKromString();
        DisplayName = br.ReadKromString();
        FlagImage = br.ReadKromString();

        FreeRideID = br.ReadUInt16();
        int count = br.ReadUInt16();

        Tracks = new Track[count];

        for (int i = 0; i < count; i++)
        {
            Tracks[i] = br.ReadIView<Track>();
        }

        Author = br.ReadKromString();
        Converter = br.ReadKromString();
        Contact = br.ReadKromString();
    }

    public override void Serialize(BinaryViewWriter bw)
    {
        bw.Write(Magic);

        bw.WriteUInt16(U0);

        bw.WriteKromString(FileName);
        bw.WriteKromString(BackgroundImage);
        bw.WriteKromString(DisplayName);
        bw.WriteKromString(FlagImage);

        bw.WriteUInt16(FreeRideID);
        bw.WriteUInt16((ushort)Tracks.Length);

        for (int i = 0; i < Tracks.Length; i++)
        {
            bw.WriteIView(Tracks[i]);
        }

        bw.WriteKromString(Author);
        bw.WriteKromString(Converter);
        bw.WriteKromString(Contact);
    }

    public class Track : IBinaryViewObject
    {
        public ushort ID;
        public ushort CheckPoint;
        public ushort Distance;
        public ushort Direction;
        public ushort WayPoint;
        public ushort TypeId;
        public ushort Sections;
        public ushort Order;
         
        public string Name;
        public string MapImage;

        public Track()
        {
            Name = string.Empty;
            MapImage = string.Empty;
        }

        public void ReadFromView(BinaryViewReader br)
        {
            ID = br.ReadUInt16();
            Name = br.ReadKromString();
            CheckPoint = br.ReadUInt16();
            Distance = br.ReadUInt16();
            Direction = br.ReadUInt16();
            WayPoint = br.ReadUInt16();
            MapImage = br.ReadKromString();
            TypeId = br.ReadUInt16();
            Sections = br.ReadUInt16();
            Order = br.ReadUInt16();
        }

        public void WriteToView(BinaryViewWriter bw)
        {
            bw.WriteUInt16(ID);
            bw.WriteKromString(Name);
            bw.WriteUInt16(CheckPoint);
            bw.WriteUInt16(Distance);
            bw.WriteUInt16(Direction);
            bw.WriteUInt16(WayPoint);
            bw.WriteKromString(MapImage);
            bw.WriteUInt16(TypeId);
            bw.WriteUInt16(Sections);
            bw.WriteUInt16(Order);
        }
    }
}
