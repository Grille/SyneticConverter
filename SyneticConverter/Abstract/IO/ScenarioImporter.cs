using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticConverter;
public abstract class ScenarioImporter
{
    protected ScenarioVariant target;
    protected string path;

    public ScenarioImporter(ScenarioVariant target)
    {
        this.target = target;
        path = target.RootDir;
    }

    public abstract void Load();
    public abstract void Assign();

    public void Load(string path)
    {
        this.path = path;
        Load();
    }

    public void LoadAndAssign()
    {
        Load();
        Assign();
    }

    public void LoadWorldTexture(string name)
    {
        string path = Path.Combine(target.RootDir, "Textures", name);
        var texture = new Texture(name);
        texture.Id = target.WorldTextures.Count;
        texture.ImportPtx(path);
        target.WorldTextures.Add(texture);
    }

    public void LoadPropTexture(string name)
    {
        string path = Path.Combine(target.RootDir, "Objects/Textures", name);
        var texture = new Texture(name);
        texture.ImportPtx(path);
        target.PropTextures.Add(texture);
    }
}
