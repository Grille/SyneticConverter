using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib;
public class TextureFolder : RessourceFolder<Texture>
{
    public void SeekPtxFiles(string path)
    {
        var files = Directory.GetFiles(path);
        foreach (var file in files)
        {
            var name = Path.GetFileNameWithoutExtension(file);
            var ext = Path.GetExtension(file).ToLower();

            if (ext == ".ptx")
            {
                Add(Texture.FromFile(path));
            }
        };
    }
}

