using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SyneticLib.Locations;


public class DirectoryLocation
{
    public string DirectoryPath { get; set; }

    public string Name => Path.GetFileName(DirectoryPath);

    public DirectoryLocation(string path)
    {
        DirectoryPath = path;
    }

    public string ChildPath(string path)
    {
        return Path.Combine(DirectoryPath, path);
    }

    public static DirectoryLocation[] FromStringArray(string[] arrray)
    {
        var result = new DirectoryLocation[arrray.Length];
        for (int i = 0; i< arrray.Length; i++)
        {
            result[i] = new DirectoryLocation(arrray[i]);
        }
        return result;
    }

    public void Seek()
    {
        OnSeek();    
    }

    protected virtual void OnSeek() { }
}
