using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Grille.IO;
using SyneticLib.Files;

namespace SyneticLib.LowLevel.Compression;
public class SynArchive
{
    record class Entry(string Name, byte[] Data);

    SynFile syn;
    List<Entry> entries;

    public SynArchive()
    {
        syn = new SynFile();
        entries = new();
    }

    public SynArchive(SynFile syn)
    {
        this.syn = syn;
        entries = new();
    }

    public void Load()
    {
        //syn.Load();

        for (int i = 0; i < syn.Entries.Length; i++)
        {
            var src = syn.Data[i];
            var dst = new byte[syn.Entries[i].TotalSize];
            //SynCompressor.Decompress(src, dst);

            entries.Add(new Entry(syn.Entries[i].FileName, dst));
        }
    }

    public void Save()
    {
        //syn.Save();
    }



    public bool FileExists(string name)
    {
        return false;
    }

    public Stream GetFile(string name)
    {
        return null;
    }

    public void SetFile(string name)
    {

    }


}
