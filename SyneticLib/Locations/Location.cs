using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.IO.Path;

namespace SyneticLib.Locations;


public abstract class Location
{
    public string Path { get; set; }

    public Location(string path)
    {
        Path = path;
    }

    public string ChildPath(string path)
    {
        return Combine(Path, path);
    }

    public void Seek()
    {
        OnSeek();    
    }

    protected abstract void OnSeek();
}
